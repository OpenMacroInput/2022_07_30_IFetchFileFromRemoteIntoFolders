using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFetchFileFromRemoteIntoFolders
{
    public void FetchFileInFolder(in string directoryWhereToDownload, in string whereToFetchTheFilePath, out bool succedWithoutError);
    public void FetchPointerInFolder(in string directoryWhereToDownload, in string whereToFetchThePointerFilePath, in FlushManagement flushType, out bool succedWithoutError);
    public enum FlushManagement { JustDownload, DeleteDirectoryFirst, Custumized}
}

public class FetchFileFromRemoteIntoFolders : IFetchFileFromRemoteIntoFolders
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


    public  void FetchFileInFolder(in string directoryWhereToDownload,
        in string whereToFetchTheFilePath,
        out bool succedWithoutError)
    {
        if (I == null)
            succedWithoutError = false;
        else
        {
            I.FetchFileInFolder(in directoryWhereToDownload, in whereToFetchTheFilePath, out succedWithoutError);
        }
    }
    public  void FetchPointerInFolder(
        in string directoryWhereToDownload,
        in string whereToFetchThePointerFilePath,
        in IFetchFileFromRemoteIntoFolders.FlushManagement flushType, 
        out bool succedWithoutError)
    {
        if (I == null)
            succedWithoutError = false;
        else
        {
            I.FetchPointerInFolder(in directoryWhereToDownload, in whereToFetchThePointerFilePath, in flushType, out succedWithoutError);
        }
    }
}

public class DefaultFetchFileFromRemoteIntoFolders : IFetchFileFromRemoteIntoFolders
{
    public IPathParserAuctioner m_parser = new DefaultPackagePathParserAuction();
    

    public void FetchFileInFolder(in string directoryWhereToDownload, in string whereToFetchTheFilePath, out bool succedWithoutError)
    {
        DownloadFileFromPathToFilePath downloader = new DownloadFileFromPathToFilePath(
            in directoryWhereToDownload, in whereToFetchTheFilePath, in m_parser);
        downloader.ProcessToDownload();
        succedWithoutError = downloader.HasDownloadSuccessfully();
    }

    public void FetchPointerInFolder(in string directoryWhereToDownload, in string whereToFetchThePointerFilePath, in IFetchFileFromRemoteIntoFolders.FlushManagement flushType, out bool succedWithoutError)
    {
        succedWithoutError = false;
        m_parser.ConvertPath(in whereToFetchThePointerFilePath, out string path);
        Debug.Log("Humm:" + path);
        if (RemoteAccessUtility.IsRelativeFilesPointerPath(in path))
        {
            DownloadFilesPointerFromPath d = new DownloadFilesPointerFromPath(in path, in directoryWhereToDownload, 
                flushType == IFetchFileFromRemoteIntoFolders.FlushManagement.DeleteDirectoryFirst);
            d.ProcessToDownload();
            succedWithoutError = d.HasDownloadSuccessfully();
            Debug.Log("HU:"+ succedWithoutError + "-"+ d.m_pointerAsTextRecovered);
        }
        else if (RemoteAccessUtility.IsWebRelativeFilesPointerPath(in path))
        {
            DownloadWebFilesPointerFromPath t = new DownloadWebFilesPointerFromPath(in path, in directoryWhereToDownload,
                flushType == IFetchFileFromRemoteIntoFolders.FlushManagement.DeleteDirectoryFirst, m_parser);
            t.ProcessToDownload();
            succedWithoutError = t.HasDownloadSuccessfully();
            Debug.Log("HD:" + succedWithoutError + "-" + t.m_recoverPointer);   
        }
    }
}
