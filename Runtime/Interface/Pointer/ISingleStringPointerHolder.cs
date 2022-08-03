using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISingleStringPointerHolder
{
    public bool IsPointerPath();
    public bool IsPointerWeblink();
    public bool CheckIfGivenPathIsExistingFile();
    public void GetStringPointerToGoFetch(out string pathOrUrlWhereAreStoreInformation);
}


public interface IFileDownloadCallbackGet
{

    public bool HasDownload();
    public void GetPathUsed(out string pathUsed);
    public void GetTextDownloaded(out string fileDownload);
    public bool HasError();
    public void GetErrorCount(out int errorCount);
    public void GetErrorsStack(out IEnumerable<string> errorsStack);
}
public interface IFileDownloadCallbackSet
{

    public void SetPathUsed(string pathUsed);
    public void SetTextDownloaded(string text);
    public void AddErrorsFound(params string[] errors);
}

[System.Serializable]
public class FileDownloadCallback : IFileDownloadCallbackGet, IFileDownloadCallbackSet
{
    [SerializeField] string m_usedPath;
    [SerializeField] string m_textDownload;
    [SerializeField] List<string> m_errorsThatHappened;
    public FileDownloadCallback()
    {
        m_usedPath = "";
        m_textDownload = "";
        m_errorsThatHappened = new List<string>();
    }
    public FileDownloadCallback(string usedPath) 
    {
        m_usedPath = usedPath;
        m_textDownload = "";
        m_errorsThatHappened = new List<string>();
    }

    public void SetPathUsed(string pathUsed) {  m_usedPath = pathUsed; }
    public void SetTextDownloaded(string text)
    {
        m_textDownload = text;
}

public void AddErrorsFound(params string [] errors) { m_errorsThatHappened.AddRange(errors);}
    

    public void GetErrorCount(out int errorCount) { errorCount = m_errorsThatHappened.Count;}
    

    public void GetErrorsStack(out IEnumerable<string> errorsStack) { errorsStack = m_errorsThatHappened;}
    

    public void GetPathUsed(out string pathUsed) { pathUsed = m_usedPath;}


    public void GetTextDownloaded(out string fileDownload) { fileDownload = m_textDownload;}

    public bool HasDownload()
    { return string.IsNullOrEmpty(m_textDownload);   }

    public bool HasError()
    {return m_errorsThatHappened.Count>0;}
}