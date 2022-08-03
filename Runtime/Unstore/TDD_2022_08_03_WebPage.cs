using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TDD_2022_08_03_WebPage : MonoBehaviour
{


    [Header("Download Pointer from webpage")]
    //http://www...github.com/F/D.relativefilepointers
    //https://raw.githubusercontent.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/main/.relativefilespointer
    public string m_pointerToPointerUrl;
    public string m_whereToStoreOnDisk;

    public WebFilesPointer m_pointerToPointerUrlWebLink;
    public string m_pointerInfoAsText;
    public string[] m_relativePathsInPointer;
    public string[] m_absolutePathsInPointer;

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        m_pointerToPointerUrlWebLink = new WebFilesPointer(m_pointerToPointerUrl);
        FileDownloadCallback pointerDownload = new FileDownloadCallback();
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
            new SingleStringPointerHolderStruct(m_pointerToPointerUrlWebLink.m_toUsePath), pointerDownload);

        if (!pointerDownload.HasError()) 
             pointerDownload.GetTextDownloaded(out m_pointerInfoAsText);
        else m_pointerInfoAsText = "";

        RemoteAccessStringUtility.SplitToLine(in m_pointerInfoAsText, out m_relativePathsInPointer);
        string webRoot = m_pointerToPointerUrlWebLink.m_toUseRootPath + "/"; 
        string localRoot = RemoteAccessStringUtility.RemoveSlashAtEnd(m_whereToStoreOnDisk) + "/";
        m_absolutePathsInPointer = m_relativePathsInPointer.Select(k => m_pointerToPointerUrlWebLink.m_toUseRootPath + "/" + k).ToArray();
        FileDownloadCallback fileDownlaod = new FileDownloadCallback();
        for (int i = 0; i < m_relativePathsInPointer.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(m_relativePathsInPointer[i]))
                continue;
            fileDownlaod.SetPathUsed(webRoot + m_relativePathsInPointer[i]);
            RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
                new SingleStringPointerHolderStruct(m_pointerToPointerUrlWebLink.m_toUsePath), fileDownlaod
                );

            if (fileDownlaod.HasError()) { }
            else
            {
                fileDownlaod.GetTextDownloaded(out string downloaded);
                string path = localRoot + m_relativePathsInPointer[i];
                string dir = Path.GetDirectoryName(path);
                Directory.CreateDirectory(dir);
                File.WriteAllText(path, downloaded);
            }
        }

    }

}
