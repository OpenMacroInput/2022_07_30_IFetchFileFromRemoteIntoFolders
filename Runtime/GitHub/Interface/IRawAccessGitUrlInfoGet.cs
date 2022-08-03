
public interface IRawAccessGitUrlInfoGet
{
    public void GetUsedUrl(out string urlPath);
    public void GetProjectName(out string project);
    public void GetAccount(out string account);
    public void GetBranch(out string branch);
    public void GetFullRelativePath(out string relativePath);
    public void GetFileWithExtension(out string fileExtension);
}


[System.Serializable]
public struct RawGitUrlInfo : IRawAccessGitUrlInfoGet
{

    public string m_gitUrl;
    public string m_project;
    public string m_account;
    public string m_fileExtension;
    public string m_relativePath;
    public string m_branch;

    public RawGitUrlInfo(string gitUrl)
    {
        string[] tokens = gitUrl.Split('/');
        m_gitUrl = gitUrl;
        //https://raw.githubusercontent.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/main/GithubPointer/DownloadMe.md
        //0//https:/
        //1///
        //2// raw.githubusercontent.com
        //3// OpenMacroInput
        //4// 2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField
        //5// main
        //6+// /GithubPointer/
        //Last// DownloadMe.md
        GitHubRemoteUtility.GetProjectNameFromPath(in
            m_gitUrl, out m_project);
        GitHubRemoteUtility.GetAccountFromPath(in
            m_gitUrl, out m_account);
        GitHubRemoteUtility.GetBranchFromPath(in
            m_gitUrl, out m_branch);

        GitHubRemoteUtility.GetRelativePath(in
            m_gitUrl, out m_relativePath);
        
        GitHubRemoteUtility.GetFileFromPath(in
            m_gitUrl, out m_fileExtension);
    }

    public void GetAccount(out string account)
    {
        account = m_account;
    }

    public void GetBranch(out string branch)
    {
        branch = m_branch;
    }

    public void GetFileWithExtension(out string fileExtension)
    {
        fileExtension = m_fileExtension;
    }

    public void GetFullRelativePath(out string relativePath)
    {
        relativePath = m_relativePath;
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