using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultFetchFileFromRemoteIntoFoldersMono : MonoBehaviour//, IFetchFileFromRemoteIntoFolders
{

    //public bool m_autoAddToStaticAccess=true;
    //public DownloadFilesPointerFromPathAndGithub m_last;
    //public void Awake()
    //{
    //    if (m_autoAddToStaticAccess) { 
    //        FetchFileFromRemoteIntoFolders.I = this;
    //    }
    //}

    //public void FetchFileInFolder(in string directoryWhereToDownload, 
    //    in string whereToFetchTheFilePath, in IFetchFileFromRemoteIntoFolders.FlushManagement flushType, out bool succedWithoutError)
    //{
    //    string target = whereToFetchTheFilePath;
    //    if(GitHubRemoteUtility.IsGitHubRelated(in whereToFetchTheFilePath))
    //        GitHubRemoteUtility.GetRawGitPathFromGitLink(in whereToFetchTheFilePath, out target);
    //    DownloadFilesPointerFromPathAndGithub p = new DownloadFilesPointerFromPathAndGithub(target, directoryWhereToDownload, flushType == IFetchFileFromRemoteIntoFolders.FlushManagement.DeleteDirectoryFirst);
    //    p.ProcessToDownload();
    //    succedWithoutError = p.HasDownloadSuccessfully();
    //    m_last = p;
    //}

    //public void FetchFileInFolder(in string directoryWhereToDownload, in string whereToFetchTheFilePath, out bool succedWithoutError)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void FetchPointerInFolder(in string directoryWhereToDownload, in string whereToFetchThePointerFilePath, in IFetchFileFromRemoteIntoFolders.FlushManagement flushType, out bool succedWithoutError)
    //{
    //    throw new System.NotImplementedException();
    //}
}
