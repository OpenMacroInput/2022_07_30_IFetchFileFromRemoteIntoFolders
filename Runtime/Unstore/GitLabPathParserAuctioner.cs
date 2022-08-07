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



