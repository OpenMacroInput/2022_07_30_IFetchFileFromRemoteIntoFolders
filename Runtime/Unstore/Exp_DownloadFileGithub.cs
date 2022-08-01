using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Exp_DownloadFileGithub : MonoBehaviour
{
    public string m_gitProject = "https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField";
    public string m_gitProjectFolder = "https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/tree/main/GithubPointer";
    public string m_gitProjectFolderFile = "https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/blob/main/GithubPointer/.urlrelativefilepointers";
    public string m_gitProjectFolderFileRaw = "https://raw.githubusercontent.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/main/GithubPointer/.urlrelativefilepointers";


    [Header("Url to created info")]
    public string m_classicGitUrlTarget;
    public RawGitUrlInfo m_rawGitUrl;
    public ClassicGitUrlInfo m_classicGitUrl;
    public HttpCloneGitUrlInfo m_httpCloneGitUrl;
    public SshCloneGitUrlInfo m_sshCloneGitUrl;

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        GitHubRemoteUtility.CreateRawGitLink(in m_classicGitUrlTarget, out string raw);
        GitHubRemoteUtility.CreateCloneGitLink(in m_classicGitUrlTarget, out string rawClone);
        GitHubRemoteUtility.CreateSshGitLink(in m_classicGitUrlTarget, out string rawSSh);
        m_classicGitUrl = new ClassicGitUrlInfo(m_classicGitUrlTarget);
        m_rawGitUrl = new RawGitUrlInfo(raw);
        m_sshCloneGitUrl = new SshCloneGitUrlInfo( rawSSh);
        m_httpCloneGitUrl = new HttpCloneGitUrlInfo(rawClone);

    }


}

public interface IClassicGitUrlInfoGet
{
    public void GetUsedUrl(out string urlPath);
    public void GetProjectName( out string project);
    public void GetAccount( out string account);
    public void GetBranch( out  string branch);
    public void GetTreetype(    out        string treeType);
    public void GetFullRelativePath( out    string relativePath);
    public void GetFileWithExtension( out    string fileExtension);
}


[System.Serializable]
public struct ClassicGitUrlInfo : IClassicGitUrlInfoGet
{

    public string m_gitUrl;
    public string m_project;
    public string m_account;
    public string m_fileExtension;
    public string m_relativePath;
    public string m_branch;
    public string m_treeType;

    public ClassicGitUrlInfo(string gitUrl)
    {
        //https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/blob/main/GithubPointer/DownloadMe.md
        //0//https:/
        //1// //
        //2// github.com
        //3// OpenMacroInput
        //4// 2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField
        //5// tree ou blob
        //5// main
        //6+// /GithubPointer/
        //Last// DownloadMe.md
            m_gitUrl = gitUrl;
        GitHubRemoteUtility.GetProjectNameFromPath(in            m_gitUrl, out m_project);
        GitHubRemoteUtility.GetAccountFromPath(in            m_gitUrl, out m_account);
        GitHubRemoteUtility.GetBranchFromPath(in            m_gitUrl, out m_branch);
        GitHubRemoteUtility.GetTreetype(in            m_gitUrl, out m_treeType);
        GitHubRemoteUtility.GetRelativePath(in            m_gitUrl, out m_relativePath);
        GitHubRemoteUtility.GetFileFromPath(in            m_gitUrl, out m_fileExtension);
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

    public void GetTreetype(out string treeType)
    {
        treeType = m_treeType;
    }

    public void GetUsedUrl(out string path)
    {
        path = m_gitUrl;
    }
}



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

public interface IShhCloneGitUrlInfoGet
{
    public void GetUsedUrl(out string urlPath);
    public void GetProjectName(out string project);
    public void GetAccount(out string account);
}

[System.Serializable]
public struct SshCloneGitUrlInfo : IShhCloneGitUrlInfoGet
{

    public string m_gitUrl;
    public string m_project;
    public string m_account;

    public SshCloneGitUrlInfo(string gitUrl)
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
public class RemotePathUtility
{

    public static bool UrlEndByFile(in string path) { GetFileExtensionWithDotOfUrl(in path, out string ext); return ext.Length > 0; }
    public static void GetFileExtensionWithDotOfUrl(in string path, out string extensionwithDot)
    {

        extensionwithDot = "";
        for (int i = path.Length - 1; i >= 0; i--)
        {
            if (path[i] == '/' || path[i] == '\\')
            {
                if (i == 0)
                {
                    return;
                }
                else
                {
                    extensionwithDot = path.Substring(i + 1);
                    return;
                }
            }
        }
    }

}

public class GitHubRemoteUtility {

    public static bool EndWithGitExtension(in string path) {
        if (path.Length < 4)
            return false;

        string s = path.ToLower();
        int l = s.Length;
        return  s[l - 4] == '.' &&
            s[l - 3] == 'g' &&
            s[l - 2] == 'i' &&
            s[l - 1] == 't'
            ;
    }
    //https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
    public static bool IsGitHubCloneLink(in string path)
    {
        string s = path.ToLower();
        return EndWithGitExtension(in path) && s.IndexOf("github.com/") > -1;
         
    }
    //git@github.com:OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
    public static bool IsGitHubShhCloneLink(in string path)
    {
        string s = path.ToLower();
        return EndWithGitExtension(in path) && s.IndexOf("github.com:") > -1;
    }
    public static bool IsPublicGitHubWebsite(in string path)
    {
        return path.ToLower().IndexOf("github.com/") > -1;
    }
    public static bool IsRawGitHubWebsite(in string path)
    {
        return path.ToLower().IndexOf("raw.githubusercontent.com/") > -1;
    }

    public static void  GetAccountFromPath(in string path, out string found) {
        string[] tokens = path.Split('/');
        if (IsPublicGitHubWebsite(in path))
        {
            if (tokens.Length > 3)
                found = tokens[3];
            else found = "";
        }
        else if (IsRawGitHubWebsite(in path)) {
            if (tokens.Length > 3)
                found = tokens[3];
            else found = "";
        }
        //https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        //git@github.com:OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        else if (IsGitHubShhCloneLink(in path) || IsGitHubCloneLink(in path))
        {
            if (tokens.Length > 0)
                found = tokens[0].Split(':')[1].Replace(".git", "" ); 
            else found = "";
        }
        else found = "";
    }

    public static void GetProjectNameFromPath(in string path, out string found)
    {
        string[] tokens = path.Split('/');
       
        //https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        //git@github.com:OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
         if (IsGitHubShhCloneLink(in path) || IsGitHubCloneLink(in path))
        {
            if (tokens.Length >0)
                found = tokens[tokens.Length-1].Replace(".git","");
            else found = "";
        }
        else if (IsPublicGitHubWebsite(in path))
        {
            if (tokens.Length > 4)
                found = tokens[4];
            else found = "";
        }
        else if (IsRawGitHubWebsite(in path))
        {
            if (tokens.Length > 4)
                found = tokens[4];
            else found = "";
        }

        else found = "";
    }
    public static void GetRelativePath(in string path, out string relativePath)
    {
        List<string> tokens = path.Split('/').ToList();
        int removeI = 0;

        if (IsPublicGitHubWebsite(in path))
            removeI = 7;
        if (IsRawGitHubWebsite(in path))
            removeI = 5;
        if (tokens.Count>= removeI) {
            for (int i = 0; i < removeI; i++)
            {
                tokens.RemoveAt(0);
            }
            relativePath = string.Join("/",tokens);
        }
        else relativePath = "";
    }
    public static void GetFileFromPath(in string path, out string found) {
        RemotePathUtility.GetFileExtensionWithDotOfUrl(in path, out found);
    }

    public static void GetTreetype(in string path, out string found)
    {
        if (IsPublicGitHubWebsite(in path))
        {
            string[] tokens = path.Split('/');
            if (tokens.Length > 5)
                found = tokens[5];
            else found = "";
        }
        else found = "";
    }

    public static void GetBranchFromPath(in string path, out string found)
    {
        string[] tokens = path.Split('/');
        if (IsPublicGitHubWebsite(in path))
        {
            if (tokens.Length > 6)
                found = tokens[6];
            else found = "";
        }
        else if (IsRawGitHubWebsite(in path))
        {
            if (tokens.Length > 5)
                found = tokens[5];
            else found = "";
        }
        else found = "";
    }

    public static void GetRawGitPathFromGitLink(in string path, out string rawPath)
    {
        if (GitHubRemoteUtility.IsRawGitHubWebsite(in path))
        {
            rawPath = path;
        }
        else if (GitHubRemoteUtility.IsPublicGitHubWebsite(in path)) { 
        
            ClassicGitUrlInfo m_classicGitUrl = new ClassicGitUrlInfo(path);
            GitHubRemoteUtility.CreateRawGitLink(m_classicGitUrl, out rawPath);
        }
        else rawPath = "";
    }

    public static void CreatePublicGitLink(in IShhCloneGitUrlInfoGet ss, out string rawPath)
    {
        ss.GetAccount(out string account);
        ss.GetProjectName(out string project);
        CreatePublicGitLink(in account, in project, out rawPath);
    }
    public static void CreatePublicGitLink(in IHttpCloneGitUrlInfoGet ss, out string rawPath)
    {
        ss.GetAccount(out string account);
        ss.GetProjectName(out string project);
        CreatePublicGitLink(in account, in project, out rawPath);

    }
    public static void CreatePublicGitLink(in string account, in string project, out string rawPath)
    { 
        //https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField
        rawPath = string.Format("https://github.com/{0}/{1}", account, project);
    }

    public static void CreateRawGitLink(in string classicGitUrl, out string rawPath)
    {
        CreateRawGitLink(new ClassicGitUrlInfo(classicGitUrl), out rawPath);
    }
    public static void CreateRawGitLink(in IClassicGitUrlInfoGet classicGitUrl, out string rawPath)
    {
        classicGitUrl.GetFullRelativePath(out string relativePath);
        if (string.IsNullOrWhiteSpace(relativePath)) { 
            rawPath = "";
            return;
        }
        classicGitUrl.GetAccount(out string acount);
        classicGitUrl.GetProjectName(out string project);
        classicGitUrl.GetBranch(out string branch);
        CreateRawGitLink(
            in acount,
            in project,
            in branch,
            in relativePath,
            out rawPath);
    }
    public static void CreateRawGitLink(in string account, in string project, in string branch, in string relativePath, out string rawPath)
    {
        // https://raw.githubusercontent.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField/main/GithubPointer/DownloadMe.md
        // https://raw.githubusercontent.com/0/1/2/3
        rawPath = string.Format("https://raw.githubusercontent.com/{0}/{1}/{2}/{3}",
            account, project, branch, relativePath);
    }

    public static void CreateCloneGitLink(in string path, out string raw)
    {
        HttpCloneGitUrlInfo clone = new HttpCloneGitUrlInfo(path);
        CreateCloneGitLink( clone, out raw);
    }

    private static void CreateCloneGitLink(in IHttpCloneGitUrlInfoGet clone, out string raw)
    {
        clone.GetAccount(out string account);
        clone.GetProjectName(out string project);
        CreateCloneGitLink(in account, in project, out raw);
    }
    public static void CreateCloneGitLink(in string account, in string project,  out string rawPath)
    {

    //    https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        rawPath = string.Format("https://github.com/{0}/{1}.git",
            account, project);
    }

    public static void CreateSshGitLink(in string path, out string raw)
    {
        SshCloneGitUrlInfo clone = new SshCloneGitUrlInfo(path);
        CreateSshGitLink( clone, out raw);
    }

    private static void CreateSshGitLink(in IShhCloneGitUrlInfoGet clone, out string raw)
    {
        clone.GetAccount(out string account);
        clone.GetProjectName(out string project);
        CreateSshGitLink(in account, in project, out raw);
    }
    public static void CreateSshGitLink(in string account, in string project,  out string rawPath)
    {
        //git@github.com:OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        rawPath = string.Format("git@github.com:{0}/{1}.git",
            account, project);
    }

}


