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
        Debug.Log("Stuf1:" + m_targetPointerPath);
        if (RemoteAccessStringUtility.IsDirectoryPath(in m_targetPointerPath)) {
            m_targetPointerPath = m_targetPointerPath + "/.relativefilespointer";
        }
        Debug.Log("Stuf2:" + m_targetPointerPath);
        m_pointerUsed = new PathToFilesPointer(m_targetPointerPath);
        m_callback = new FileDownloadCallback();
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
            new SingleStringPointerHolderStruct(m_pointerUsed.m_toUsePath), m_callback);
        m_pointerRecovered = !m_callback.HasError();
        if (m_pointerRecovered)
            m_callback.GetTextDownloaded(out m_pointerAsTextRecovered);
        else m_pointerAsTextRecovered = "";
        Debug.Log("Stuf3:" + m_pointerAsTextRecovered);

        if (m_pointerRecovered)
        {
            if (m_flushFolderOnDiskBeforeDownloading)
            {
               RemoteAccessUtility.TryToDeleteAllFilesInDirectory(m_whereToStoreOnDisk);
            }
        }

        RemoteAccessStringUtility.SplitToLine(in m_pointerAsTextRecovered, out m_relativePathsInPointer);
        m_relativePathsInPointer = m_relativePathsInPointer.Where(k => !string.IsNullOrEmpty(k)).ToArray();
        m_webRoot = RemoteAccessStringUtility.RemoveSlashAtEnd(m_pointerUsed.m_toUseRootPath) + "/";
        m_localRoot = RemoteAccessStringUtility.RemoveSlashAtEnd(m_whereToStoreOnDisk) + "/";
        m_absolutePathsFromGivenRoot = m_relativePathsInPointer.Select(k => m_webRoot + k).ToArray();
        FileDownloadCallback fileDownloadd = new FileDownloadCallback();
        m_absolutePathsStored = new string[m_relativePathsInPointer.Length];
        for (int i = 0; i < m_relativePathsInPointer.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(m_relativePathsInPointer[i]))
                continue;
            if (RemoteAccessStringUtility.IsFilePath(in m_relativePathsInPointer[i])) { 
                fileDownloadd.SetPathUsed(m_webRoot + m_relativePathsInPointer[i]);
                RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
                    new SingleStringPointerHolderStruct(m_pointerUsed.m_toUsePath), fileDownloadd
                    );

                if (fileDownloadd.HasError())
                {  }
                else
                {
                    fileDownloadd.GetTextDownloaded(out string downloaded);
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
                    Debug.Log("Humdm:" + path+":");
                    }
            }
        }
        m_hadLaunchDownload = true;
    }

   
}

[System.Serializable]
public class RelativeFilePointer
{
    public List<RelativeFilePointerItem> m_items = new List<RelativeFilePointerItem>();
}
[System.Serializable]
public class RelativeFilePointerItem
{
    public string m_relativePath;
}







[System.Serializable]
public class DownloadFileFromPathToFilePath
{

    [Header("Enter Info")]
    public string m_filePath;
    public string m_whereToStoreOnDisk;
    public IPathParserAuctioner m_urlParser;

    [Header("Process Log Info")]
    public FileDownloadCallback m_callback;
    public bool m_fileRecovered;
    public string m_fileTextRecovered;
    public string m_localRoot;
    public string m_absolutePathsStored;
    private bool m_hasLaunchDownload;
    private bool m_hadLaunchDownload;

    public DownloadFileFromPathToFilePath(in string targetPointerPath, in string whereToStoreOnDisk, in IPathParserAuctioner parser = null)
    {
        m_filePath = targetPointerPath;
        m_whereToStoreOnDisk = whereToStoreOnDisk;
        m_urlParser = parser;
    }
    public virtual void Set(in string pointerPath, in string whereToStoreOnDiskPath, in IPathParserAuctioner parser=null)
    {
        m_filePath = pointerPath;
        m_whereToStoreOnDisk = whereToStoreOnDiskPath;
        m_urlParser = parser;
    }
    public bool HasDownloadSuccessfully() { return m_hasLaunchDownload && m_hadLaunchDownload && !m_callback.HasError(); }
    public void Clear()
    {

        m_callback = null;
        m_fileRecovered = false;
        m_fileTextRecovered = "";
        m_localRoot = "";
        m_absolutePathsStored = "";
        m_hasLaunchDownload = false;
        m_hadLaunchDownload = false;
    }

    public void ProcessToDownload()
    {
        if (m_whereToStoreOnDisk == null || m_whereToStoreOnDisk.Length <= 0)
            return;
        Clear();
        m_hadLaunchDownload = false;
        m_hasLaunchDownload = true;
        if (m_urlParser != null) { 
            m_urlParser.ConvertPath(m_filePath, out m_filePath);
        }

        m_callback = new FileDownloadCallback();
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
            new SingleStringPointerHolderStruct(m_filePath), m_callback);
        m_fileRecovered = !m_callback.HasError();
        if (m_fileRecovered)
            m_callback.GetTextDownloaded(out m_fileTextRecovered);
        else m_fileTextRecovered = "";

        if (RemoteAccessStringUtility.IsFilePath(in m_whereToStoreOnDisk))
        {
            string dir = Path.GetDirectoryName(m_whereToStoreOnDisk);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(m_whereToStoreOnDisk, m_fileTextRecovered);
            m_hadLaunchDownload = true;
            return;
        }
        else if (RemoteAccessStringUtility.IsDirectoryPath(in m_whereToStoreOnDisk))
        {
             RemoteAccessStringUtility.GetLastSlashSegmentOfPath(in m_filePath, out string lastSegment);
            if (lastSegment != null && lastSegment.Length <= 0) { 
                string targetStore = RemoteAccessStringUtility.RemoveSlashAtEnd(m_whereToStoreOnDisk) + "/" + lastSegment;

                string dir = Path.GetDirectoryName(targetStore);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(targetStore, m_fileTextRecovered);
                m_hadLaunchDownload = true;
                return;
            }
        }
        m_hadLaunchDownload = true;
    }


}
