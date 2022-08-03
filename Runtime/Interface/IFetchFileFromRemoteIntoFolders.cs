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
    public static IFetchFileFromRemoteIntoFolders I;
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
