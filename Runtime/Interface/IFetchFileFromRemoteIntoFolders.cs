using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFetchFileFromRemoteIntoFolders
{
    public void FetchFileInFolder(in string directoryWhereToDownload, in string whereToFetchTheFilePath, in FetchFileFlushManagement flushType, out bool succedWithoutError);
    public enum FetchFileFlushManagement { JustDownload, DeleteAllDirectoryAndReload, Custumized}
}

public class FetchFileFromRemoteIntoFolders
{
    public static IFetchFileFromRemoteIntoFolders m_instance;
    public static IFetchFileFromRemoteIntoFolders I {
         get
        {
            if (m_instance == null) m_instance = new DefaultFetchFileFromRemoteIntoFolders();
            return m_instance;
        }
         set { m_instance =value ; }
    }


    public static void FetchFileInFolder(in string directoryWhereToDownload, 
        in string whereToFetchTheFilePath,
        in IFetchFileFromRemoteIntoFolders.FetchFileFlushManagement flushType,
        out bool succedWithoutError)
    {
        if (I == null)
            succedWithoutError = false;
        else {
            I.FetchFileInFolder(in directoryWhereToDownload, in whereToFetchTheFilePath, in flushType, out succedWithoutError);
        }
    }
}

public class DefaultFetchFileFromRemoteIntoFolders : IFetchFileFromRemoteIntoFolders
{
    public void FetchFileInFolder(in string directoryWhereToDownload, in string whereToFetchTheFilePath, in IFetchFileFromRemoteIntoFolders.FetchFileFlushManagement flushType, out bool succedWithoutError)
    {
        DownloadFilesPointerFromPathAndGithub d = new DownloadFilesPointerFromPathAndGithub(in whereToFetchTheFilePath, in directoryWhereToDownload, true);
        d.ProcessToDownload();
        succedWithoutError = d.HasDownloadSuccessfully();
    }
}
