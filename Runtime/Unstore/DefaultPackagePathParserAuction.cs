public class DefaultPackagePathParserAuction : PathParserAuctioner
{
    public PathParserAuctioner[] m_auctioners = new PathParserAuctioner[] {
        new GistPathParserAuctioner(),
        new GitLabPathParserAuctioner(),
        new GitHubPathParserAuctioner()
        };

    public override bool CanUnderstand(in string path)
    {
        for (int i = 0; i < m_auctioners.Length; i++)
        {
            if (m_auctioners[i].CanUnderstand(in path))
            {
                return true;
            }
        }
        return false;
    }

    public override bool ConvertPath(in string path, out string newPath) {
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




public interface IPathParserAuctioner
{

    public bool CanUnderstand(in string path);
    public bool ConvertPath(in string path, out string newPath);

}
public abstract class PathParserAuctioner : IPathParserAuctioner
{

    public abstract bool CanUnderstand(in string path);
    public abstract bool ConvertPath(in string path, out string newPath);

}

