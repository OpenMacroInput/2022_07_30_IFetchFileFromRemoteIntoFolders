using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchFilesFromPointer_ToUnityAssets : AbstractRelativeFetchFilesFromPointerMono
{


    public override string GetAbstractPath()
    {
        return Application.dataPath;
    }
   
}
public abstract class AbstractRelativeFetchFilesFromPointerMono : MonoBehaviour
{
    public string m_relativePath;
    public string m_pathWhatToDownload;
    public bool m_deleteFolderWhenDownload;
    public DownloadFilesPointerFromPathAndGithub m_downloadLog;
    public string m_debugPathThatWillBeUsed;
  
    public abstract string GetAbstractPath();
    public string GetAbsoluteWithRelative() {
        return RemoteAccessStringUtility.RemoveSlashAtEnd(GetAbstractPath()) + "/" + RemoteAccessStringUtility.RemoveSlashAtStart(m_relativePath);
    }
    [ContextMenu("Download")]
    public void Download() {

        string dirPath = GetAbsoluteWithRelative();
        m_downloadLog = new DownloadFilesPointerFromPathAndGithub(in m_pathWhatToDownload, in dirPath, in m_deleteFolderWhenDownload);
    }
    [ContextMenu("Refesh Path Debug")]
    private void OnValidate()
    {
        m_debugPathThatWillBeUsed = GetAbsoluteWithRelative();
    }
}
