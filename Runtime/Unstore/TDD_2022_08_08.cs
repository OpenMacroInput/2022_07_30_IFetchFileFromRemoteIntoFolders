using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_2022_08_08 : MonoBehaviour
{
    public string m_whereToStore;
    public string m_pathGiven;
    public string m_pathConverted;
    public bool m_succedWithoutError;
   
    [ContextMenu("Convert Path")]
    public void ConvertPath()
    {
        FetchFileFromRemoteIntoFolders.I.FetchPointerInFolder(
            in m_whereToStore, in m_pathGiven,
             IFetchFileFromRemoteIntoFolders.FlushManagement.DeleteDirectoryFirst, out m_succedWithoutError);
    }
}

