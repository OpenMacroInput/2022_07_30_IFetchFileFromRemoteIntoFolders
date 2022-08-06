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

    public PathParserAuction auction = new PathParserAuction();

    //https://gist.githubusercontent.com/OpenMacroInput/11150aa9a549e5e8f3e93ad6502d30a7/raw/c97c09df4e03c7a69e758aad9c0a71dc9d768f53/HelloFirstGist.webrelativefilespointer
    [ContextMenu("Convert Path")]
    public void ConvertPath()
    {
        auction.ConvertPath(in m_pathGist, out m_pathGistConverted);
        auction.ConvertPath(in m_pathGitLab, out m_pathGitLabConverted);
        auction.ConvertPath(in m_pathGitHub, out m_pathGitHubConverted);
    }
}


public class PathParserAuction 
{
    public PathParserAuctioner[] m_auctioners = new PathParserAuctioner[] {
        new GistPathParserAuctioner(),
        new GitLabPathParserAuctioner(),
        new GitHubPathParserAuctioner()
        };


    public bool ConvertPath(in string path, out string newPath) {

        newPath = path;
        for (int i = 0; i < m_auctioners.Length; i++)
        {
            if (m_auctioners[i].CanUnderstand(in path)) {
                return m_auctioners[i].ConvertPath(in path, out newPath);
                
            }
        }
        return false;
    }
}
public interface  IPathParserAuctioner
{

    public  bool CanUnderstand(in string path);
    public  bool ConvertPath(in string path, out string newPath);

}
public abstract class PathParserAuctioner: IPathParserAuctioner
{

    public abstract bool CanUnderstand(in string path);
    public abstract bool ConvertPath(in string path, out string newPath);

}

public class GitHubPathParserAuctioner : PathParserAuctioner
{
    public override bool CanUnderstand(in string path)
    {
        return GitHubRemoteUtility.IsGitHubRelated(in path);
    }

    public override bool ConvertPath(in string path, out string newPath)
    {
        if (GitHubRemoteUtility.IsGitHubRelated(in path))
        {
            GitHubRemoteUtility.GetRawGitPathFromGitLink(in path, out newPath);
            return true;
        }
        else {
            newPath = path;
            return false;
        }
    }
}
public class GitLabPathParserAuctioner : PathParserAuctioner
{
    public override bool CanUnderstand(in string path)
    {
        return GitLabRemoteUtility.IsGitLabRelated(in path);
    }

    public override bool ConvertPath(in string path, out string newPath)
    {
        if (GitLabRemoteUtility.IsGitLabRelated(in path))
        {
            GitLabRemoteUtility.GetRawGitPathFromGitLink(in path, out newPath);
            return true;
        }
        else
        {
            newPath = path;
            return false;
        }
    }
}


public class GistPathParserAuctioner : PathParserAuctioner
{
    public override bool CanUnderstand(in string path)
    {
        return GistHubRemoteUtility.IsGistRelated(in path);
    }

    public override bool ConvertPath(in string path, out string newPath)
    {
        if (GistHubRemoteUtility.IsGistRelated(in path))
        {
            GistHubRemoteUtility.GetRawGitPathFromGitLink(in path, out newPath);
            return true;
        }
        else
        {
            newPath = path;
            return false;
        }
    }
}


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
        gistRawLink = gistLink.Replace("gist.github.com/", "gist.githubusercontent.com/").Replace("#file-", "/raw/").Replace("-",".");
        
    }
}