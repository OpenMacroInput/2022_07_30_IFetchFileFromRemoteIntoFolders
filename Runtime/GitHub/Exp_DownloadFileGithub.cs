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
        GitHubRemoteUtility.CreateRawGitLinkFromClassic(in m_classicGitUrlTarget, out string raw);
        GitHubRemoteUtility.CreateCloneGitLink(in m_classicGitUrlTarget, out string rawClone);
        GitHubRemoteUtility.CreateSshGitLink(in m_classicGitUrlTarget, out string rawSSh);
        m_classicGitUrl = new ClassicGitUrlInfo(m_classicGitUrlTarget);
        m_rawGitUrl = new RawGitUrlInfo(raw);
        m_sshCloneGitUrl = new SshCloneGitUrlInfo( rawSSh);
        m_httpCloneGitUrl = new HttpCloneGitUrlInfo(rawClone);

    }


}




