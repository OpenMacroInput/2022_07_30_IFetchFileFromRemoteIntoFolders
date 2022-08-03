using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFetchFileFromRemoteIntoFoldersMono : MonoBehaviour, IFetchFileFromRemoteIntoFolders
{

    public bool m_autoAddToStaticAccess=true;
    public DownloadFilesPointerFromPath m_last;
    public void Awake()
    {
        if (m_autoAddToStaticAccess) { 
            FetchFileFromRemoteIntoFolders.I = this;
        }
    }

    public void FetchFileInFolder(in string directoryWhereToDownload, in string whereToFetchTheFilePath, in IFetchFileFromRemoteIntoFolders.FetchFileFlushManagement flushType, out bool succedWithoutError)
    {
        string target = whereToFetchTheFilePath;
        if(GitHubRemoteUtility.IsGitHubRelated(in whereToFetchTheFilePath))
            GitHubRemoteUtility.GetRawGitPathFromGitLink(in whereToFetchTheFilePath, out target);
        DownloadFilesPointerFromPath p = new DownloadFilesPointerFromPath(target, directoryWhereToDownload, flushType == IFetchFileFromRemoteIntoFolders.FetchFileFlushManagement.DeleteAllDirectoryAndReload);
        p.ProcessToDownload();
        succedWithoutError = p.HasDownloadSuccessfully();
        m_last = p;
    }
}
