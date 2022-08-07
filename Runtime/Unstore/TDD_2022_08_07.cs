using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_2022_08_07 : MonoBehaviour
{
    public string m_pathGiven;
    public string m_pathConverted;

    public string m_pathGist = "https://gist.github.com/OpenMacroInput/11150aa9a549e5e8f3e93ad6502d30a7#file-hellofirstgist-webrelativefilespointer";
    public string m_pathGistConverted;
    public string m_pathGitLab = "https://gist.github.com/OpenMacroInput/11150aa9a549e5e8f3e93ad6502d30a7#file-hellofirstgist-webrelativefilespointer";
    public string m_pathGitLabConverted;
    public string m_pathGitHub = "https://gist.github.com/OpenMacroInput/11150aa9a549e5e8f3e93ad6502d30a7#file-hellofirstgist-webrelativefilespointer";
    public string m_pathGitHubConverted;

    public DefaultPackagePathParserAuction auction = new DefaultPackagePathParserAuction();

    //https://gist.githubusercontent.com/OpenMacroInput/11150aa9a549e5e8f3e93ad6502d30a7/raw/c97c09df4e03c7a69e758aad9c0a71dc9d768f53/HelloFirstGist.webrelativefilespointer
    [ContextMenu("Convert Path")]
    public void ConvertPath()
    {
        auction.ConvertPath(in m_pathGist, out m_pathGistConverted);
        auction.ConvertPath(in m_pathGitLab, out m_pathGitLabConverted);
        auction.ConvertPath(in m_pathGitHub, out m_pathGitHubConverted);
        auction.ConvertPath(in m_pathGiven, out m_pathConverted);
    }
}



