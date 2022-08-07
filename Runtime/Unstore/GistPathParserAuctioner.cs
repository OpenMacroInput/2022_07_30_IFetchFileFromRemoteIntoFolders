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



