using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class FetchFilesFromPointer_ToUnityAssets : AbstractRelativeFetchFilesFromPointerMono
{


    public override string GetAbstractPath()
    {
        return Application.dataPath;
    }

    protected override void DownloadNotification()
    {
#if UNITY_EDITOR

        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
public abstract class AbstractRelativeFetchFilesFromPointerMono : MonoBehaviour
{
    public bool m_autoDownloadAtAwake=true;
    public string m_relativePath;
    public string m_pathWhatToDownload;
    public bool m_deleteFolderWhenDownload;
    public DownloadFilesPointerFromPathAndGithub m_downloadLog;
    public string m_debugPathThatWillBeUsed;

    public UnityEvent m_launched;
    public UnityEvent m_succed;
    public UnityEvent m_failed;
    private void Awake()
    {
        if (m_autoDownloadAtAwake)
            Download();
    }

    public abstract string GetAbstractPath();
    public string GetAbsoluteWithRelative() {
        return RemoteAccessStringUtility.RemoveSlashAtEnd(GetAbstractPath()) + "/" + RemoteAccessStringUtility.RemoveSlashAtStart(m_relativePath);
    }

    [ContextMenu("Create directory")]
    public void CreateDirectory() {

        string d = Path.GetDirectoryName(GetAbsoluteWithRelative());
        Directory.CreateDirectory(d);
        Directory.CreateDirectory(GetAbsoluteWithRelative());
    }

    [ContextMenu("Download")]
    public void Download() {
        m_launched.Invoke();
        string dirPath = GetAbsoluteWithRelative();
        m_downloadLog = new DownloadFilesPointerFromPathAndGithub(in m_pathWhatToDownload, in dirPath, in m_deleteFolderWhenDownload);
        m_downloadLog.ProcessToDownload();
        bool dl = m_downloadLog.HasDownloadSuccessfully() && !m_downloadLog.m_callback.HasError();
        if (dl)
            m_succed.Invoke();
        else m_failed.Invoke();

    }

    protected abstract void DownloadNotification();

    [ContextMenu("Refesh Path Debug")]
    private void OnValidate()
    {
        m_debugPathThatWillBeUsed = GetAbsoluteWithRelative();
    }

    [ContextMenu("Open Url")]
    public void OpenUrl() {

        Application.OpenURL(GetAbsoluteWithRelative());
    
    }
}
