using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TDD_2022_08_02 : MonoBehaviour
{
  

    [Header("Download Pointer from Github")]
    //http://www...github.com/F/D.relativefilepointers
    //https://raw.githubusercontent.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/main/.relativefilespointer
    public string m_pointerToGithubPointerUrl;
    public string m_whereToStoreOnDisk;

    public RawGitUrlInfo m_infoRaw;
    public ClassicGitUrlInfo m_infoClassic;
    public string m_githubPointerUrlRaw;
    public WebFilesPointer m_pointerToGithubPointerUrlWebLink;
    public string m_pointerInfoAsText;
    public string[] m_relativePathsInPointer;
    public string[] m_absolutePathsInPointer;

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        m_infoRaw = new RawGitUrlInfo(m_pointerToGithubPointerUrl);
        m_infoClassic = new ClassicGitUrlInfo(m_pointerToGithubPointerUrl);
        GitHubRemoteUtility.CreateRawGitLinkFromClassic(m_infoClassic, out m_githubPointerUrlRaw);
        m_pointerToGithubPointerUrlWebLink = new WebFilesPointer(m_githubPointerUrlRaw);
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
            new SingleStringPointerHolderStruct(m_pointerToGithubPointerUrlWebLink.m_toUsePath), FetchRelativeInfo);

    }

    private void FetchRelativeInfo(string text, bool succedToDownlaod)
    {
        if (succedToDownlaod) m_pointerInfoAsText = text;
        else m_pointerInfoAsText = "";
        RemoteAccessStringUtility.SplitToLine(in m_pointerInfoAsText, out m_relativePathsInPointer);
        string webRoot = m_pointerToGithubPointerUrlWebLink.m_toUseRootPath + "/"
         ;string localRoot = RemoteAccessStringUtility.RemoveSlashAtEnd(m_whereToStoreOnDisk) + "/";
        m_absolutePathsInPointer = m_relativePathsInPointer.Select(k => m_pointerToGithubPointerUrlWebLink.m_toUseRootPath + "/" + k).ToArray();
        FileDownloadCallback fileDownlaod = new FileDownloadCallback();
        for (int i = 0; i < m_relativePathsInPointer.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(m_relativePathsInPointer[i]))
                continue;
            fileDownlaod.SetPathUsed(webRoot + m_relativePathsInPointer[i]);
            RemoteAccessUtility.DownloadFileAsText_CSharpClassic(
                new SingleStringPointerHolderStruct(m_pointerToGithubPointerUrlWebLink.m_toUsePath), fileDownlaod
                );

            if (fileDownlaod.HasError() ) { }
            else {
                fileDownlaod.GetTextDownloaded(out string downloaded);
                string path = localRoot + m_relativePathsInPointer[i];
                string dir = Path.GetDirectoryName(path);
                Directory.CreateDirectory(dir);
                File.WriteAllText(path, downloaded);
            }
        }
    }
}

[System.Serializable]
public class PathToFilesPointer {

    public string m_toUsePath;
    public string m_toUseRootPath;
    public PathToFilesPointer(string toUsePath)
    {
        m_toUsePath = toUsePath;
        GetRootOfPath(in toUsePath, out m_toUseRootPath);
    }
    public static void GetRootOfPath(in string path, out string rootOfPath)
    {
        int index = path.LastIndexOf('\\');
        if (index < 0) index = path.LastIndexOf('/');
        if (index < 0)
            throw new System.Exception("That path don't have root");
        rootOfPath = path.Substring(0, index);
    }
}

[System.Serializable]
public class DiskFilesPointer : PathToFilesPointer
{
    public DiskFilesPointer(string toUsePath) : base(toUsePath)
    {
    }
}
[System.Serializable]
public class WebFilesPointer : PathToFilesPointer
{
    public WebFilesPointer(string toUsePath) : base(toUsePath)
    {
    }
}
[System.Serializable]
public class GithubFilesPointer : PathToFilesPointer
{
    public GithubFilesPointer(string toUsePath) : base(toUsePath)
    {
    }
}