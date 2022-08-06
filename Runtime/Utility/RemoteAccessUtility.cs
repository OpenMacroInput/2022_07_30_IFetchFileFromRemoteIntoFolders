
using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteAccessUtility
{

    public delegate void RemoteCallback(string text, bool succedToDownlaod);
    public delegate void RemoteCallbackWithError(string text, int errorCount, string[] errorMessages);

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
    public static IEnumerator DownloadFileAsText_Coroutine(ISingleStringPointerHolder source, RemoteCallback callback)
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
                    text = www.downloadHandler.text;
                    downloadSuccessfully = true;
                }
            }
        }
        if (callback != null)
            callback(text, downloadSuccessfully);
        yield break;


    }
    public static void DownloadFileAsText_CSharpClassic(in ISingleStringPointerHolder source, in IFileDownloadCallbackSet callback)
    {

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
            //Debug.LogWarning("Exception:" + e.StackTrace);
            callback.AddErrorsFound(e.StackTrace);
        }
        callback.SetPathUsed(path);
        callback.SetTextDownloaded(text);
    }

    public static void DownloadFileAsText_CSharpClassic(in ISingleStringPointerHolder source, RemoteCallback callback)
    {
        string text = "";
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
                text = client.DownloadString(path);
                downloadSuccessfully = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Exception:" + e.StackTrace);
        }
        if (callback != null)
            callback(text, downloadSuccessfully);
    }

    public static void TryToDeleteAllFilesInDirectory(in string whereToStoreOnDisk)
    {
        try
        {
            if (Directory.Exists(whereToStoreOnDisk))
                Directory.Delete(whereToStoreOnDisk,true);
        }
        catch (Exception) { }
        try
        {
            if (Directory.Exists(whereToStoreOnDisk))
            {
                string[] files = Directory.GetFiles(whereToStoreOnDisk);
                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i]);
                }
                string[] directories = Directory.GetDirectories(whereToStoreOnDisk);
                for (int i = 0; i < directories.Length; i++)
                {
                    Directory.Delete(directories[i]);
                }
                Directory.Delete(whereToStoreOnDisk);
            }
        }
        catch (Exception) { }
        Directory.CreateDirectory(whereToStoreOnDisk);
    }
}
