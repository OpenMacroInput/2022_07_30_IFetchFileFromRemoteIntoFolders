using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitLabRemoteUtility
{

    public static bool IsGitLabRelated(in string path)
    {
        return path.ToLower().IndexOf("gitlab.com") >= 0;
    }
    public static void GetRawGitPathFromGitLink(in string gitLabLink, out string gitLabRawLink)
    {
        //From
        //https://gitlab.com/eloistree/2019_08_30_PostItVR/-/blob/master/Runtime/CodeAndQuests.md
        //To
        //https://gitlab.com/eloistree/2019_08_30_PostItVR/-/raw/master/Runtime/CodeAndQuests.md

        gitLabRawLink = gitLabLink.Replace("/blob/", "/raw/");
    }
}
