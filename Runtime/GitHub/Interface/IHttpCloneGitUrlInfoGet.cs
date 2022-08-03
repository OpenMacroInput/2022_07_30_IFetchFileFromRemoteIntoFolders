

public interface IHttpCloneGitUrlInfoGet
{
    public void GetUsedUrl(out string urlPath);
    public void GetProjectName(out string project);
    public void GetAccount(out string account);
}

[System.Serializable]
public struct HttpCloneGitUrlInfo : IHttpCloneGitUrlInfoGet
{

    public string m_gitUrl;
    public string m_project;
    public string m_account;

    public HttpCloneGitUrlInfo(string gitUrl)
    {
        m_gitUrl = gitUrl;
        GitHubRemoteUtility.GetProjectNameFromPath(in
            m_gitUrl, out m_project);
        GitHubRemoteUtility.GetAccountFromPath(in
            m_gitUrl, out m_account);
    }

    public void GetAccount(out string account)
    {
        account = m_account;
    }

    public void GetProjectName(out string project)
    {
        project = m_project;
    }

    public void GetUsedUrl(out string urlPath)
    {
        urlPath = m_gitUrl;
    }
}
