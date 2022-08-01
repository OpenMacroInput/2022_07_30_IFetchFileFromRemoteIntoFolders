using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class TDD_2022_01_08 : MonoBehaviour
{



    [Header("Download file by delegate")]
    public SingleStringPointerHolderStruct m_downloadFileCoroutineUrl;
    public string m_downloadFileCoroutine;
    public SingleStringPointerHolderStruct m_downloadFileCSharpUrl;
    public string m_downloadFileCSharp;
    [Header("Download file by class callback")]
    public SingleStringPointerHolderStruct m_urlRelativeFilePointerUrl
        = new SingleStringPointerHolderStruct("https://raw.githubusercontent.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/main/GithubPointer/.urlrelativefilepointers");
    public FileDownloadCallback m_urlRelativeFilePointer;
    public SingleStringPointerHolderStruct m_relativeFilePointerUrl
        = new SingleStringPointerHolderStruct("https://raw.githubusercontent.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/main/FilePointA/FilePointer.relativefilepointers");
    public FileDownloadCallback m_relativeFiePointer;


    IEnumerator Start()
    {
        yield return RemoteAccessUtility.DownloadFileAsText_Coroutine(m_downloadFileCoroutineUrl, Coro_Callback);
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(m_downloadFileCSharpUrl, CS_CallBack);
    }

    [ContextMenu("Refresh")]
    public void Refresh() {

        StartCoroutine(RemoteAccessUtility.DownloadFileAsText_Coroutine(m_urlRelativeFilePointerUrl, m_urlRelativeFilePointer));
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(m_relativeFilePointerUrl, m_relativeFiePointer);

    }

    
    private void Coro_Callback(string text, bool succedToDownload)
    {
        m_downloadFileCoroutine = text;
    }

    private void CS_CallBack(string text, bool succedToDownload)
    {
        m_downloadFileCSharp = text;
    }
   
}

[System.Serializable]
public struct SingleStringPointerHolderStruct : ISingleStringPointerHolder
{
    [SerializeField] string m_givenPath;

    public SingleStringPointerHolderStruct(string givenPath)
    {
        m_givenPath = givenPath;
    }

    public bool CheckIfGivenPathIsExistingFile()
    {
       return File.Exists(m_givenPath);
    }

    public void GetStringPointerToGoFetch(out string pathOrUrlWhereAreStoreInformation)
    {
        pathOrUrlWhereAreStoreInformation = m_givenPath;
    }
    public bool IsPointerPath()
    {
        return !IsPointerWeblink();
    }
    public bool IsPointerWeblink()
    {
        if (m_givenPath.Length>5 && 
            (m_givenPath[0] == 'H' || m_givenPath[0] == 'h') &&
            (m_givenPath[1] == 'T' || m_givenPath[1] == 't') &&
            (m_givenPath[2] == 'T' || m_givenPath[2] == 't') &&
            (m_givenPath[3] == 'P' || m_givenPath[3] == 'p') &&
            (m_givenPath[4] == ':')) return true;
        if (m_givenPath.Length > 4 && 
            (m_givenPath[0] == 'W' || m_givenPath[0] == 'w') &&
            (m_givenPath[1] == 'W' || m_givenPath[1] == 'w') &&
            (m_givenPath[2] == 'W' || m_givenPath[2] == 'w') &&
            (m_givenPath[3] == '.')) return true;
        return false;
    }
}

[System.Serializable]
public class TestDownloadPointersHandler_FromWebsite : IDownloadPointersHandler_FromWebsite
{
    public void DownloadFileFrom(in ILocalDirectoryRoot directoryRootToDownload, in ISingleStringPointerHolder urlOfFileToDownlaod, out bool succedToDownload)
    {
        throw new System.NotImplementedException();
    }

    public void Handle(in ILocalDirectoryRoot directoryRootToDownload, in ISingleStringPointerHolder urlRootOfWebsite, in IDirectoriesConstructorFile pointerInfo, out int errorCount, out string[] errorMessage)
    {
        throw new System.NotImplementedException();
    }

    public void Handle(in ILocalDirectoryRoot directoryRootToDownload, in ISingleStringPointerHolder urlRootOfWebsite, in IRelativeFilePointers pointerInfo, out int errorCount, out string[] errorMessage)
    {
        throw new System.NotImplementedException();
    }

    public void Handle(in ILocalDirectoryRoot directoryRootToDownload, in ISingleStringPointerHolder urlRootOfWebsite, in IUrlRelativeFilePointers pointerInfo, out int errorCount, out string[] errorMessage)
    {
        throw new System.NotImplementedException();
    }
}


public class RemoteAccessUtility {

    public delegate void RemoteCallback( string text,  bool succedToDownlaod);
    public delegate void RemoteCallbackWithError( string text,  int errorCount,  string [] errorMessages);

    public static void DownloadFileAsText_Coroutine(MonoBehaviour mono, ISingleStringPointerHolder source, IFileDownloadCallbackSet callback)
    {
        mono.StartCoroutine(DownloadFileAsText_Coroutine(source, callback));
    }
    public static IEnumerator DownloadFileAsText_Coroutine(ISingleStringPointerHolder source, IFileDownloadCallbackSet callback)
    {
        string text = "";
        source.GetStringPointerToGoFetch(out string path);
        if (source.CheckIfGivenPathIsExistingFile())
        {
            //Should I catch the exception?
            text = File.ReadAllText(path);
        }
        else
        {
            using (UnityWebRequest www = UnityWebRequest.Get(path))
            {
                yield return www.SendWebRequest();
                if (www.error != null && string.IsNullOrEmpty(www.error))
                {
                   //Debug.LogWarning(www.error);
                    callback.AddErrorsFound(www.error);
                }
                else
                {
                    text = www.downloadHandler.text;
                }
            }
        }
        callback.SetPathUsed(path);
        callback.SetTextDownloaded(text);
        yield break;
    }
    public static void DownloadFileAsText_Coroutine(MonoBehaviour mono, ISingleStringPointerHolder source, RemoteCallback callback)
    {
        mono.StartCoroutine(DownloadFileAsText_Coroutine(source, callback));
    }
    public static IEnumerator DownloadFileAsText_Coroutine( ISingleStringPointerHolder source, RemoteCallback callback)
    {
            string text = "";
            bool downloadSuccessfully = false;
            source.GetStringPointerToGoFetch(out string path);
            if (source.CheckIfGivenPathIsExistingFile())
            {
                //Should I catch the exception?
                text = File.ReadAllText(path);
                downloadSuccessfully = true;
            }
            else
            {
                using (UnityWebRequest www = UnityWebRequest.Get(path))
                {
                    yield return www.SendWebRequest();
                    if (www.error != null && string.IsNullOrEmpty(www.error)) 
                    {
                        Debug.LogWarning(www.error);
                    }
                    else
                    {
                        text= www.downloadHandler.text;
                        downloadSuccessfully = true;
                    }
                }
            }
            if (callback != null)
                callback(text, downloadSuccessfully);
            yield break;
        
        
    }
    public static void DownloadFileAsText_CSharpClassic(in ISingleStringPointerHolder source, IFileDownloadCallbackSet callback) {

        string text = "";
        source.GetStringPointerToGoFetch(out string path);
        try
        {
            if (source.CheckIfGivenPathIsExistingFile())
            {
                text = File.ReadAllText(path);
            }
            else
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                text = client.DownloadString(path);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Exception:" + e.StackTrace);
            callback.AddErrorsFound(e.StackTrace);
        }
        callback.SetPathUsed(path);
        callback.SetTextDownloaded(text);
    }

    public static void DownloadFileAsText_CSharpClassic(in ISingleStringPointerHolder source, RemoteCallback callback)
    {
        string text="";
        bool downloadSuccessfully = false;
        try
        {
            source.GetStringPointerToGoFetch(out string path);
            if (source.CheckIfGivenPathIsExistingFile())
            {
                text = File.ReadAllText(path);
                downloadSuccessfully = true;
            }
            else
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                text= client.DownloadString(path);
                downloadSuccessfully = true;
            }
        }
        catch (Exception e) {
            Debug.LogWarning("Exception:" + e.StackTrace);
        }
        if(callback!=null)
            callback(text, downloadSuccessfully);
    }

}