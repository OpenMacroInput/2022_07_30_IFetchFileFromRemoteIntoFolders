using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GistHubRemoteUtility
{

    public static bool IsGistRelated(in string path)
    {
        return path.ToLower().IndexOf("gist.github.com/") >= 0;
    }
    public static void GetRawGitPathFromGitLink(in string gistLink, out string gistRawLink)
    {
        //From
        //https://gist.github.com/OpenMacroInput/11150aa9a549e5e8f3e93ad6502d30a7#file-hellofirstgist-webrelativefilespointer
        //To 
        //https://gist.githubusercontent.com/OpenMacroInput/11150aa9a549e5e8f3e93ad6502d30a7/raw/HelloFirstGist.webrelativefilespointer
        //
        gistRawLink = gistLink.Replace("gist.github.com/", "gist.githubusercontent.com/").Replace("#file-", "/raw/").Replace("-", ".");

    }
}