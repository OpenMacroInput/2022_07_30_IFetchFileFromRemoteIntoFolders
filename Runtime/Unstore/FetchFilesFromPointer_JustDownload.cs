using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchFilesFromPointer_JustDownload : MonoBehaviour
{
    public string m_target;
    public string m_directory;
    public bool m_succedToDownload;

    [ContextMenu("Fetch")]
    public void Fetch() {
        FetchFileFromRemoteIntoFolders.I.FetchFileInFolder(in m_directory, in m_target, IFetchFileFromRemoteIntoFolders.FetchFileFlushManagement.JustDownload, out m_succedToDownload);

    }
}
