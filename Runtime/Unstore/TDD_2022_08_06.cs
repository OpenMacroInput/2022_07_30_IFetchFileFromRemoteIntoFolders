
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TDD_2022_08_06 : MonoBehaviour
{
    [Header("In")]
    public string m_pointerPath;
    public string m_whereToStoreDirectory;
    public bool m_deleteFolderToFlush;
    public DownloadWebFilesPointerFromPathAndGithub m_pointer;



    [ContextMenu("Download")]
    public void Download()
    {
        m_pointer.Set(in m_pointerPath, in m_whereToStoreDirectory, in m_deleteFolderToFlush);
        m_pointer.Download();
    }
}



[System.Serializable]
public class DownloadWebFilesPointerFromPathAndGithub : DownloadWebFilesPointerFromPath
{
    public DownloadWebFilesPointerFromPathAndGithub(in string targetPointerPath, in string whereToStoreOnDisk, in bool flushFolderOnDiskBeforeDownloading) : base(in targetPointerPath, in whereToStoreOnDisk, in flushFolderOnDiskBeforeDownloading)
    {
        this.Set(in targetPointerPath, in whereToStoreOnDisk, in flushFolderOnDiskBeforeDownloading);
    }

    public override void Set(in string pointerPath, in string whereToStoreOnDiskPath, in bool deleteFolderBeforeDownload)
    {
        string newTarget = pointerPath;
        if (GitHubRemoteUtility.IsGitHubRelated(in newTarget))
        {
            if (!RemoteAccessStringUtility.IsFilePath(in newTarget))
            {
                newTarget = RemoteAccessStringUtility.RemoveSlashAtEnd(newTarget) + "/.webrelativefilespointer";
            }
            GitHubRemoteUtility.GetRawGitPathFromGitLink(in newTarget, out newTarget);
        }
        base.Set(newTarget, whereToStoreOnDiskPath, in deleteFolderBeforeDownload);
    }
}



[System.Serializable]
public class DownloadWebFilesPointerFromPath
{
    [Header("In")]
    public string m_pointerPath;
    public string m_whereToStoreDirectory;
    public bool m_deleteFolderToFlush;
    [Header("Debug")]
    public string m_usedPointerPath;
    public bool m_pointerRecovered;
    public FileDownloadCallback m_callback;
    public string m_recoverPointer;
    public List<WebRelativeFilePointerItem> m_items;



    public DownloadWebFilesPointerFromPath(in string targetPointerPath, in string whereToStoreOnDisk, in bool flushFolderOnDiskBeforeDownloading)
    {
        m_pointerPath = targetPointerPath;
        m_whereToStoreDirectory = whereToStoreOnDisk;
        m_deleteFolderToFlush = flushFolderOnDiskBeforeDownloading;
    }
    public virtual void Set(in string pointerPath, in string whereToStoreOnDiskPath, in bool deleteFolderBeforeDownload)
    {
        m_pointerPath = pointerPath;
        m_whereToStoreDirectory = whereToStoreOnDiskPath;
        m_deleteFolderToFlush = deleteFolderBeforeDownload;
    }


    [ContextMenu("Download")]
    public void Download() {

        if(m_deleteFolderToFlush)
            RemoteAccessUtility.TryToDeleteAllFilesInDirectory(in m_whereToStoreDirectory);

        m_callback = new FileDownloadCallback();
        m_usedPointerPath = m_pointerPath;
        if (GitHubRemoteUtility.IsGitHubRelated(in m_pointerPath))
        {
            GitHubRemoteUtility.GetRawGitPathFromGitLink(in m_pointerPath, out m_usedPointerPath);
        }

        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
            new SingleStringPointerHolderStruct(m_usedPointerPath), m_callback);
        m_pointerRecovered = !m_callback.HasError();
        if (m_pointerRecovered)
            m_callback.GetTextDownloaded(out m_recoverPointer);
        else m_recoverPointer = "";

        if (m_deleteFolderToFlush && Directory.Exists(m_whereToStoreDirectory)) { 
            Directory.Delete(m_whereToStoreDirectory);
            Directory.CreateDirectory(m_whereToStoreDirectory);
        }
            

        WebRelativeFilePointerUtility.GetItemsWithText(in m_recoverPointer, out m_items);
        WebRelativeFilePointerGitHubUtility.ConvertToRawGitIfLinkedToGit(in m_items);
        WebRelativeFilePointerUtility.Download(in m_whereToStoreDirectory, in m_items);
    }
}


public class WebRelativeFilePointerGitHubUtility {

    public static void ConvertToRawGitIfLinkedToGit(in WebRelativeFilePointerItem item)
    {
        if (GitHubRemoteUtility.IsGitHubRelated(in item.m_webUrlWhereToDownload)) {
            GitHubRemoteUtility.GetRawGitPathFromGitLink(in item.m_webUrlWhereToDownload, out string s);
            item.m_webUrlWhereToDownload = s;
        }
    }
    public static void ConvertToRawGitIfLinkedToGit(in List<WebRelativeFilePointerItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (GitHubRemoteUtility.IsGitHubRelated(in items[i].m_webUrlWhereToDownload))
            {
                GitHubRemoteUtility.GetRawGitPathFromGitLink(in items[i].m_webUrlWhereToDownload, out string s);
                items[i].m_webUrlWhereToDownload = s;
            }
        }
    }
}
public class WebRelativeFilePointerUtility
{
    public static void GetItemsWithText(in string text, out List<WebRelativeFilePointerItem> items)
    {
        GetItemsWithText(text.Split('\n'), out items);
    }
    public static void GetItemsWithText(in string[] texts, out List<WebRelativeFilePointerItem> items)
    {
        items = new List<WebRelativeFilePointerItem>();
        List<string> lines = new List<string>();
        for (int i = 0; i < texts.Length; i++)
        {
            lines.AddRange(texts[i].Split('\n'));
        }
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i] = lines[i].Trim();
            string lineLow = lines[i].ToLower();
            if (lineLow.IndexOf("https:") >= 0 || lineLow.IndexOf("http:") >= 0)
            {
                WebRelativeFilePointerItem item = new WebRelativeFilePointerItem();
                WebRelativeFilesPointerUtility.SplitInDouble(lines[i], out item.m_relativePath, out item.m_webUrlWhereToDownload);
               
                item.m_relativePath = item.m_relativePath.Trim();
                item.m_webUrlWhereToDownload = item.m_webUrlWhereToDownload.Trim();
                items.Add(item);
            }
        }
    }

    public static void Download(in string storageDirectory, in List<WebRelativeFilePointerItem> items)
    {
        foreach (WebRelativeFilePointerItem item in items)
        {
            Download(in storageDirectory, in item);
        }
    }
    public static void Download(in string storageDirectory, in WebRelativeFilePointerItem item)
    {
       
        if (storageDirectory == null || storageDirectory.Length <= 0
            || item == null || item.m_webUrlWhereToDownload == null || item.m_webUrlWhereToDownload.Length <= 0) {
            return;
        }

      
        FileDownloadCallback callback = new FileDownloadCallback();
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
            new SingleStringPointerHolderStruct(item.m_webUrlWhereToDownload), callback
            );

        if (RemoteAccessStringUtility.IsDirectoryPath(in item.m_relativePath))
        {
            if (callback.HasError()) { }
            else
            {
                callback.GetTextDownloaded(out string downloaded);
                RemoteAccessStringUtility.GetLastSlashSegmentOfPath(in item.m_webUrlWhereToDownload, out string lastFileSegment);
                
                if (lastFileSegment.Length > 0)
                {
                    string pathWithRelative = RemoteAccessStringUtility.MergePathWithSlash(storageDirectory, item.m_relativePath);
                    if (lastFileSegment.IndexOf('.') >= 0)
                    {
                        string path = "";
                        string dir = "";
                        path = RemoteAccessStringUtility.MergePathWithSlash(pathWithRelative, lastFileSegment); 
                        if (path != null && path.Length > 0)
                        {
                            dir = Path.GetDirectoryName(path);
                            try
                            {
                                Directory.CreateDirectory(dir);
                                File.WriteAllText(path, downloaded);
                            }
                            catch
                            {
                                Debug.LogWarning("DD##" + path);
                            }
                        }
                    }
                   
                    
                }
            }
        }
        else if (RemoteAccessStringUtility.IsFilePath(in item.m_relativePath))
        {
            string absolutePath = RemoteAccessStringUtility.MergePathWithSlash(storageDirectory, item.m_relativePath);
            if (callback.HasError()){ }
            else
            {
                callback.GetTextDownloaded(out string downloaded);
                string dir = Path.GetDirectoryName(absolutePath);
                try
                {
                    Directory.CreateDirectory(dir);
                    File.WriteAllText(absolutePath, downloaded);
                }
                catch
                {
                    Debug.LogWarning("Can't create##" + dir + "##" + absolutePath);
                }
            }
        }
    }
}




[System.Serializable]
public class WebRelativeFilePointer {
    public List<WebRelativeFilePointerItem> m_items= new List<WebRelativeFilePointerItem>();
}
[System.Serializable]
public class WebRelativeFilePointerItem
{
    public string m_relativePath;
    public string m_webUrlWhereToDownload;
}


[System.Serializable]
public class WebRelativeFilePointerItemToAbsoluteDirectory
{
    public string m_whereToStoreDirectory;
    public WebRelativeFilePointerItem m_toFetchInfo;
}
[System.Serializable]
public class WebRelativeFilePointerToAbsoluteDirectory
{
    public string m_whereToStoreDirectory;
    public WebRelativeFilePointer m_toFetchInfo;
}




[System.Serializable]
public class DownloadWebRelativeFilesPointerFromPathAndGithub : DownloadWebRelativeFilesPointerFromPath
{
    public DownloadWebRelativeFilesPointerFromPathAndGithub(in string targetPointerPath, in string whereToStoreOnDisk, in bool flushFolderOnDiskBeforeDownloading) : base(in targetPointerPath, in whereToStoreOnDisk, in flushFolderOnDiskBeforeDownloading)
    {
        this.Set(in targetPointerPath, in whereToStoreOnDisk, in flushFolderOnDiskBeforeDownloading);
    }

    public override void Set(in string pointerPath, in string whereToStoreOnDiskPath, in bool deleteFolderBeforeDownload)
    {
        string newTarget = pointerPath;
        if (GitHubRemoteUtility.IsGitHubRelated(in newTarget))
        {
            if (!RemoteAccessStringUtility.IsFilePath(in newTarget))
            {
                newTarget = RemoteAccessStringUtility.RemoveSlashAtEnd(newTarget) + "/.webrelativefilespointer";
            }
            GitHubRemoteUtility.GetRawGitPathFromGitLink(in newTarget, out newTarget);
        }
        base.Set(newTarget, whereToStoreOnDiskPath, in deleteFolderBeforeDownload);
    }

}

[System.Serializable]
public class DownloadWebRelativeFilesPointerFromPath
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
    public WebRelativeFilePointer m_foundPointer= new WebRelativeFilePointer();
    public string[] m_linesInFile;
    public string[] m_absolutePathsFromGivenRoot;
    public string[] m_absolutePathsStored;
    private bool m_hasLaunchDownload;
    private bool m_hadLaunchDownload;

    public DownloadWebRelativeFilesPointerFromPath(in string targetPointerPath, in string whereToStoreOnDisk, in bool flushFolderOnDiskBeforeDownloading)
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
        m_linesInFile = new string[0]; ;
        m_absolutePathsFromGivenRoot = new string[0]; ;
        m_absolutePathsStored = new string[0];
        m_hasLaunchDownload = false;
        m_hadLaunchDownload = false;
        m_foundPointer = new WebRelativeFilePointer();
    }

    public void ProcessToDownload()
    {
        //Clear();
        //m_hasLaunchDownload = true;
        //if (RemoteAccessStringUtility.IsDirectoryPath(in m_targetPointerPath))
        //{
        //    m_targetPointerPath = m_targetPointerPath + "/.webrelativefilespointer";
        //}
        //m_pointerUsed = new PathToFilesPointer(m_targetPointerPath);
        //m_callback = new FileDownloadCallback();
        //RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
        //    new SingleStringPointerHolderStruct(m_targetPointerPath), m_callback);
        //m_pointerRecovered = !m_callback.HasError();
        //if (m_pointerRecovered)
        //    m_callback.GetTextDownloaded(out m_pointerAsTextRecovered);
        //else m_pointerAsTextRecovered = "";

        //if (m_pointerRecovered)
        //{
        //    if (m_flushFolderOnDiskBeforeDownloading)
        //    {
        //        RemoteAccessUtility.TryToDeleteAllFilesInDirectory(m_whereToStoreOnDisk);
        //    }
        //}

        //RemoteAccessStringUtility.SplitToLine(in m_pointerAsTextRecovered, out m_linesInFile);
        //m_linesInFile = m_linesInFile.Where(k => !string.IsNullOrEmpty(k)).ToArray();
        //m_webRoot = m_pointerUsed.m_toUseRootPath + "/";
        //m_localRoot = RemoteAccessStringUtility.RemoveSlashAtEnd(m_whereToStoreOnDisk) + "/";
        ////m_foundPointer.SetWithLines(m_linesInFile);

        //FileDownloadCallback fileDownlaod = new FileDownloadCallback();
        //m_absolutePathsFromGivenRoot = new string[m_foundPointer.m_items.Count];
        //m_absolutePathsStored = new string[m_foundPointer.m_items.Count];
        //for (int i = 0; i < m_foundPointer.m_items.Count; i++)
        //{
            
        //    WebRelativeFilePointerItem item = m_foundPointer.m_items[i];
        //    string absolutePath = RemoteAccessStringUtility.MergePathWithSlash( m_pointerUsed.m_toUseRootPath , item.m_relativePath);
        //    m_absolutePathsFromGivenRoot[i] = absolutePath;
        //    if (absolutePath==null || absolutePath.Length<=0 || item.m_webUrlWhereToDownload == null || item.m_webUrlWhereToDownload.Length <= 0)
        //        continue;
           
        //    if (RemoteAccessStringUtility.IsDirectoryPath(in absolutePath) && RemoteAccessStringUtility.IsFilePath(in item.m_webUrlWhereToDownload))
        //    {
        //        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
        //            new SingleStringPointerHolderStruct(item.m_webUrlWhereToDownload), fileDownlaod
        //            );

        //        if (fileDownlaod.HasError()) { }
        //        else
        //        {
        //            fileDownlaod.GetTextDownloaded(out string downloaded);
        //            RemoteAccessStringUtility.GetLastSlashSegmentOfPath(in item.m_webUrlWhereToDownload, out string extension) ;
        //            if (extension.Length > 0) 
        //            { 
        //                string path = RemoteAccessStringUtility.RemoveSlashAtEnd(absolutePath)+"/"+ extension;
        //                string dir = Path.GetDirectoryName(path);
        //                m_absolutePathsStored[i] = path;
        //                //try {
        //                Directory.CreateDirectory(dir);
        //                File.WriteAllText(path, downloaded);
        //                //}catch { }
        //            }
        //        }
        //    }
        //    else if (RemoteAccessStringUtility.IsFilePath(in absolutePath))
        //    {
        //        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
        //            new SingleStringPointerHolderStruct(item.m_webUrlWhereToDownload), fileDownlaod
        //            );

        //        if (fileDownlaod.HasError())
        //        { }
        //        else
        //        {
        //            fileDownlaod.GetTextDownloaded(out string downloaded);
        //            string path = absolutePath;
        //            string dir = Path.GetDirectoryName(path);
        //            m_absolutePathsStored[i] = path;
        //            try {
        //                Directory.CreateDirectory(dir);
        //                File.WriteAllText(path, downloaded);
        //            }
        //            catch
        //            {
        //                Debug.LogWarning("Can't create##" + dir+ "##" + path);
        //            }
        //        }
        //    }
        //}
        //m_hadLaunchDownload = true;
    }


}
