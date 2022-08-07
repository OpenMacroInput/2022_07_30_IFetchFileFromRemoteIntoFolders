using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchFilesFromPointer_InHashIdFolder : MonoBehaviour
{
    public string m_target;
    public string m_directory;
    public bool m_succedToDownload;

    [ContextMenu("Fetch")]
    public void Fetch()
    {
        long id = GenerateIdFrom(in m_target);
        string dir = RemoteAccessStringUtility.RemoveSlashAtEnd(m_directory) + "/" + id;
        FetchFileFromRemoteIntoFolders.I.FetchPointerInFolder(in dir, in m_target, IFetchFileFromRemoteIntoFolders.FlushManagement.JustDownload, out m_succedToDownload);
    }

    private long GenerateIdFrom(in string target)
    {
        long l = 0;
        for (int i = 0; i < target.Length; i++)
        {
           l+=(int)target[i];
        }return l;
    }
}
