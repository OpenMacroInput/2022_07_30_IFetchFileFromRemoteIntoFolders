using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IBean_RelativeStorage { 

    public void GetRelativePathWhereToStore(out string path);
}

public interface IBean_CreateDirectoryAtLocation : IBean_RelativeStorage {

    public void GetRelativeDirectoryPathToCreate(out string relativeDirecotyr);
}

public interface IBean_DirectLinkToDownloadAtLocation : IBean_RelativeStorage
{
    public void GetDirectLinkToDownlaodPath(out string directLink);
}
public interface IBean_GetContextRootRelativeFileAtLocation : IBean_RelativeStorage
{
    public void GetFileRelativePathWithoutContext(out string relativeFilePath);
}


public class Bean_CreateDirecotry
{
    public string m_relativeDirectoryPathToCreate;
}
public class Bean_CreateFileFromDirectLink
{
    public string m_relativeFilePathWhereToDownload;
    public string m_directLink;
}
public class Bean_ContextRootRelativeFile
{
    public string m_relativeFilePathWhereToDownload;
    public string m_relativePathToUseInContext;
}