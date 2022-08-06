using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GitHubRemoteUtility
{

    public static bool EndWithGitExtension(in string path)
    {
        if (path.Length < 4)
            return false;

        string s = path.ToLower();
        int l = s.Length;
        return s[l - 4] == '.' &&
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

    public static void GetAccountFromPath(in string path, out string found)
    {
        string[] tokens = path.Split('/');
        if (IsPublicGitHubWebsite(in path))
        {
            if (tokens.Length > 3)
                found = tokens[3];
            else found = "";
        }
        else if (IsRawGitHubWebsite(in path))
        {
            if (tokens.Length > 3)
                found = tokens[3];
            else found = "";
        }
        //https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        //git@github.com:OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        else if (IsGitHubShhCloneLink(in path) || IsGitHubCloneLink(in path))
        {
            if (tokens.Length > 0)
                found = tokens[0].Split(':')[1].Replace(".git", "");
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
            if (tokens.Length > 0)
                found = tokens[tokens.Length - 1].Replace(".git", "");
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
        if (tokens.Count >= removeI)
        {
            for (int i = 0; i < removeI; i++)
            {
                tokens.RemoveAt(0);
            }
            relativePath = string.Join("/", tokens);
        }
        else relativePath = "";
    }
    public static void GetFileFromPath(in string path, out string found)
    {
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
        else if (GitHubRemoteUtility.IsPublicGitHubWebsite(in path))
        {
            ClassicGitUrlInfo m_classicGitUrl = new ClassicGitUrlInfo(path);
            Debug.Log(string.Format("{0}--{1}--{2}--{3}--{4}--{5}",
                m_classicGitUrl.m_account,
                m_classicGitUrl.m_project,
                m_classicGitUrl.m_fileExtension,
                m_classicGitUrl.m_branch,
                m_classicGitUrl.m_relativePath,
                m_classicGitUrl.m_treeType));
            GitHubRemoteUtility.CreateRawGitLinkFromClassic(m_classicGitUrl, out rawPath);
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

    public static void CreateRawGitLinkFromClassic(in string classicGitUrl, out string rawPath)
    {
        CreateRawGitLinkFromClassic(new ClassicGitUrlInfo(classicGitUrl), out rawPath);
    }
    public static void CreateRawGitLinkFromClassic(in IClassicGitUrlInfoGet classicGitUrl, out string rawPath)
    {
        classicGitUrl.GetFullRelativePath(out string relativePath);
        //if (string.IsNullOrWhiteSpace(relativePath))
        //{
        //    rawPath = "";
        //    return;
        //}
        //classicGitUrl.GetUsedUrl(out string url);
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
            account, project, branch.Length>0?branch: "main", RemoteAccessStringUtility.RemoveSlashAtStart( relativePath));




    }

    public static void CreateCloneGitLink(in string path, out string raw)
    {
        HttpCloneGitUrlInfo clone = new HttpCloneGitUrlInfo(path);
        CreateCloneGitLink(clone, out raw);
    }

    private static void CreateCloneGitLink(in IHttpCloneGitUrlInfoGet clone, out string raw)
    {
        clone.GetAccount(out string account);
        clone.GetProjectName(out string project);
        CreateCloneGitLink(in account, in project, out raw);
    }
    public static void CreateCloneGitLink(in string account, in string project, out string rawPath)
    {

        //    https://github.com/OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        rawPath = string.Format("https://github.com/{0}/{1}.git",
            account, project);
    }

    public static void CreateSshGitLink(in string path, out string raw)
    {
        SshCloneGitUrlInfo clone = new SshCloneGitUrlInfo(path);
        CreateSshGitLink(clone, out raw);
    }

    private static void CreateSshGitLink(in IShhCloneGitUrlInfoGet clone, out string raw)
    {
        clone.GetAccount(out string account);
        clone.GetProjectName(out string project);
        CreateSshGitLink(in account, in project, out raw);
    }
    public static void CreateSshGitLink(in string account, in string project, out string rawPath)
    {
        //git@github.com:OpenMacroInput/2022_07_30_IFetchFileFromRemoteIntoFolders_TrainingField.git
        rawPath = string.Format("git@github.com:{0}/{1}.git",
            account, project);
    }

    public static bool IsGitHubRelated(in string path)
    {
        return IsPublicGitHubWebsite(in path) || IsRawGitHubWebsite(in path) || IsGitHubShhCloneLink(in path) || IsGitHubCloneLink(in path);
    }
}
