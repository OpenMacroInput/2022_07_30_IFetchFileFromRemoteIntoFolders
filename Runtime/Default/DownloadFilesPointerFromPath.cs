using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DownloadFilesPointerFromPathAndGithub : DownloadFilesPointerFromPath
{
    public DownloadFilesPointerFromPathAndGithub(in string targetPointerPath, in string whereToStoreOnDisk, in bool flushFolderOnDiskBeforeDownloading) : base(in targetPointerPath, in whereToStoreOnDisk, in flushFolderOnDiskBeforeDownloading)
    {
        this.Set(in targetPointerPath, in whereToStoreOnDisk, in flushFolderOnDiskBeforeDownloading);
    }

    public override void Set(in string pointerPath, in string whereToStoreOnDiskPath, in bool deleteFolderBeforeDownload)
    {   
        string newTarget = pointerPath;
        if (GitHubRemoteUtility.IsGitHubRelated(in newTarget)) {
            if (!RemoteAccessStringUtility.IsFilePath(in newTarget)) { 
                newTarget = RemoteAccessStringUtility.RemoveSlashAtEnd(newTarget) + "/.relativefilespointer";
            }
            GitHubRemoteUtility.GetRawGitPathFromGitLink(in newTarget, out newTarget);
        }
        base.Set(newTarget, whereToStoreOnDiskPath, in deleteFolderBeforeDownload);
    }

}

[System.Serializable]
public class DownloadFilesPointerFromPath
{

    [Header("Enter Info")]
    public string m_targetPointerPath;
    public string m_whereToStoreOnDisk;
    public bool m_flushFolderOnDiskBeforeDownloading;

    [Header("Process Log Info")]
    public PathToFilesPointer m_pointerUsed;
    public FileDownloadCallback m_callback;
    public bool m_pointerRecovered;
    public string m_pointerAsTextRecovered;
    public string m_webRoot;
    public string m_localRoot;
    public string[] m_relativePathsInPointer;
    public string[] m_absolutePathsFromGivenRoot;
    public string[] m_absolutePathsStored;
    private bool m_hasLaunchDownload;
    private bool m_hadLaunchDownload;

    public DownloadFilesPointerFromPath(in string targetPointerPath, in string whereToStoreOnDisk, in bool flushFolderOnDiskBeforeDownloading)
    {
        m_targetPointerPath = targetPointerPath;
        m_whereToStoreOnDisk = whereToStoreOnDisk;
        m_flushFolderOnDiskBeforeDownloading = flushFolderOnDiskBeforeDownloading;
    }
    public virtual void Set(in string pointerPath, in string whereToStoreOnDiskPath, in bool deleteFolderBeforeDownload)
    {
        m_targetPointerPath = pointerPath;
        m_whereToStoreOnDisk = whereToStoreOnDiskPath;
        m_flushFolderOnDiskBeforeDownloading = deleteFolderBeforeDownload;
    }
    public bool HasDownloadSuccessfully() { return m_hasLaunchDownload && m_hadLaunchDownload && !m_callback.HasError(); }
    public void Clear()
    {

        m_pointerUsed = null;
        m_callback = null;
        m_pointerRecovered = false;
        m_pointerAsTextRecovered = "";
        m_webRoot = "";
        m_localRoot = "";
        m_relativePathsInPointer = new string[0]; ;
        m_absolutePathsFromGivenRoot = new string[0]; ;
        m_absolutePathsStored = new string[0];
        m_hasLaunchDownload = false;
        m_hadLaunchDownload = false;
    }

    public void ProcessToDownload()
    {
        Clear();
        m_hasLaunchDownload = true;
        if (RemoteAccessStringUtility.IsDirectoryPath(in m_targetPointerPath)) {
            m_targetPointerPath = m_targetPointerPath + "/.relativefilespointer";
        }
        m_pointerUsed = new PathToFilesPointer(m_targetPointerPath);
        m_callback = new FileDownloadCallback();
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
            new SingleStringPointerHolderStruct(m_pointerUsed.m_toUsePath), m_callback);
        m_pointerRecovered = !m_callback.HasError();
        if (m_pointerRecovered)
            m_callback.GetTextDownloaded(out m_pointerAsTextRecovered);
        else m_pointerAsTextRecovered = "";

        if (m_pointerRecovered)
        {
            if (m_flushFolderOnDiskBeforeDownloading)
            {
               RemoteAccessUtility.TryToDeleteAllFilesInDirectory(m_whereToStoreOnDisk);
            }
        }

        RemoteAccessStringUtility.SplitToLine(in m_pointerAsTextRecovered, out m_relativePathsInPointer);
        m_relativePathsInPointer = m_relativePathsInPointer.Where(k => !string.IsNullOrEmpty(k)).ToArray();
        m_webRoot = m_pointerUsed.m_toUseRootPath + "/";
        m_localRoot = RemoteAccessStringUtility.RemoveSlashAtEnd(m_whereToStoreOnDisk) + "/";
        m_absolutePathsFromGivenRoot = m_relativePathsInPointer.Select(k => m_pointerUsed.m_toUseRootPath + "/" + k).ToArray();
        FileDownloadCallback fileDownlaod = new FileDownloadCallback();
        m_absolutePathsStored = new string[m_relativePathsInPointer.Length];
        for (int i = 0; i < m_relativePathsInPointer.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(m_relativePathsInPointer[i]))
                continue;
            if (RemoteAccessStringUtility.IsFilePath(in m_relativePathsInPointer[i])) { 
                fileDownlaod.SetPathUsed(m_webRoot + m_relativePathsInPointer[i]);
                RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
                    new SingleStringPointerHolderStruct(m_pointerUsed.m_toUsePath), fileDownlaod
                    );

                if (fileDownlaod.HasError())
                {  }
                else
                {
                    fileDownlaod.GetTextDownloaded(out string downloaded);
                    string path = m_localRoot + m_relativePathsInPointer[i];
                    string dir = Path.GetDirectoryName(path);
                    m_absolutePathsStored[i] = path;
                    //try {
                    Directory.CreateDirectory(dir);
                    File.WriteAllText(path, downloaded);
                    //}catch { }
                }
            }
            else if (RemoteAccessStringUtility.IsDirectoryPath(in m_relativePathsInPointer[i]))
            {
                    string path = m_localRoot + m_relativePathsInPointer[i];
                    m_absolutePathsStored[i] = path;
                    try {
                    if(!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                    }catch {
                    Debug.Log("Humm:" + path+":");
                    }
            }
        }
        m_hadLaunchDownload = true;
    }

   
}
