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



