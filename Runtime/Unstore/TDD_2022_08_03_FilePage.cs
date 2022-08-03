using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TDD_2022_08_03_FilePage : MonoBehaviour
{
    public string m_targetPointerPath;
    public string m_whereToStoreOnDisk;
    public bool m_flushFolderOnDiskBeforeDownloading;
    public DownloadFilesPointerFromPath m_pathDownloader;
    [ContextMenu("Process to download")]
    public void ProcessToDownload()
    {
        m_pathDownloader = new DownloadFilesPointerFromPath(
            m_targetPointerPath,
            m_whereToStoreOnDisk,
            m_flushFolderOnDiskBeforeDownloading
            );
        m_pathDownloader.ProcessToDownload();
    }
    [ContextMenu("Clear")]
    public void Clear() {
        m_pathDownloader.Clear();
    }
}
[System.Serializable]
public class DownloadFilesPointerFromGithubPath
{
    public string m_givenPath;
    public string m_convertedPath;

    public DownloadFilesPointerFromPath m_webPath;

}
