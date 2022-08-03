using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


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

    public DownloadFilesPointerFromPath(in string targetPointerPath, in string whereToStoreOnDisk, in bool flushFolderOnDiskBeforeDownloading)
    {
        m_targetPointerPath = targetPointerPath;
        m_whereToStoreOnDisk = whereToStoreOnDisk;
        m_flushFolderOnDiskBeforeDownloading = flushFolderOnDiskBeforeDownloading;
    }
    public void Set(in string pointerPath, in string whereToStoreOnDiskPath, in bool deleteFolderBeforeDownload)
    {
        m_targetPointerPath = pointerPath;
        m_whereToStoreOnDisk = whereToStoreOnDiskPath;
        m_flushFolderOnDiskBeforeDownloading = deleteFolderBeforeDownload;
    }
    public bool HasDownloadSuccessfully() { return m_absolutePathsStored.Length > 0; }
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
    }

    public void ProcessToDownload()
    {
        Clear();
        m_pointerUsed = new DiskFilesPointer(m_targetPointerPath);
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
                TryToDeleteAllFilesInDirectory(m_whereToStoreOnDisk);
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
            fileDownlaod.SetPathUsed(m_webRoot + m_relativePathsInPointer[i]);
            RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
                new SingleStringPointerHolderStruct(m_pointerUsed.m_toUsePath), fileDownlaod
                );

            if (fileDownlaod.HasError())
            {
            }
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

    }

    private void TryToDeleteAllFilesInDirectory(in string pathOfDirectoryToFlush)
    {
        try
        {
            if (Directory.Exists(m_whereToStoreOnDisk))
                Directory.Delete(m_whereToStoreOnDisk);
        }
        catch (Exception) { }
        try
        {
            if (Directory.Exists(m_whereToStoreOnDisk))
            {
                string[] files = Directory.GetFiles(m_whereToStoreOnDisk);
                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i]);
                }
                string[] directories = Directory.GetDirectories(m_whereToStoreOnDisk);
                for (int i = 0; i < directories.Length; i++)
                {
                    Directory.Delete(directories[i]);
                }
                Directory.Delete(m_whereToStoreOnDisk);
            }
        }
        catch (Exception) { }
        Directory.CreateDirectory(m_whereToStoreOnDisk);
    }
}
