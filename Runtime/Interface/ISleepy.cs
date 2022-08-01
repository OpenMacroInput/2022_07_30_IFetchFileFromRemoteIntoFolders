using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The idea here is to have a file that can points to others files around.
/// </summary>
public interface IProjectRootPointersPath
{
    public void GetDirecotriesConstructorPath(in ISingleStringPointerHolder constructorPath);
    public void GetRelativeFilePointerPath(in ISingleStringPointerHolder constructorPath);
    public void GetUrlRelativeFilePointerPath(in ISingleStringPointerHolder constructorPath);
}

public interface IRelativeFilePointersRootBuilder {
    public void CreatePathPoint(in string directoryRoot, out IProjectRootPointersPath pointer);
    public void CreatePathPoint(in string directoryRoot, out IDirectoriesConstructorFile pointer);
    public void CreatePathPoint(in string directoryRoot, out IRelativeFilePointers pointer);
    public void CreatePathPoint(in string directoryRoot, out IUrlRelativeFilePointers pointer);
}

public interface IDownloadPointersHandler_FromWebsite
{
    public void Handle(
         in ILocalDirectoryRoot directoryRootToDownload,    
         in ISingleStringPointerHolder urlRootOfWebsite,
         in IDirectoriesConstructorFile pointerInfo,
         out int errorCount, out string[] errorMessage);
    public void Handle(
         in ILocalDirectoryRoot directoryRootToDownload,
         in ISingleStringPointerHolder urlRootOfWebsite,
         in IRelativeFilePointers pointerInfo,
         out int errorCount, out string[] errorMessage);
    public void Handle(
         in ILocalDirectoryRoot directoryRootToDownload,
         in ISingleStringPointerHolder urlRootOfWebsite,
         in IUrlRelativeFilePointers pointerInfo,
         out int errorCount, out string[] errorMessage);

    public void DownloadFileFrom(
       in ILocalDirectoryRoot directoryRootToDownload,
       in ISingleStringPointerHolder urlOfFileToDownlaod,
       out bool succedToDownload);

}


public interface IDownloadPointers_FromPublicGithub
{
    public void DownloadFrom(
        in ILocalDirectoryRoot directoryRootToDownload,
        in ISingleStringPointerHolder urlRootOfWebsite,
        in IProjectRootPointersPath pointerInfo,
        out int errorCount, out string[] errorMessage);
    public void DownloadFileFrom(
       in ILocalDirectoryRoot directoryRootToDownload,
       in ISingleStringPointerHolder urlOfFileToDownlaod,
       out bool succedToDownload);
}

public interface ILocalDirectoryRoot {

    public void GetDirectoryPath(out string path);
    public bool IsDirectoryExists();
    public void CreateDirectoryIfNotExisting();
}


/////////////////
/// <summary>
/// Flat means that we don't have information about directory structure, just where is the file to download
/// </summary>
public interface IUrlFlatFilePointers
{
    public void GetUrlPointers(out string[] urlPathOfTheFileToDownload);
    public void GetUrlFilePathCount(out bool count);
}

///////////////////////////
///

// For Open Macro Input, I want the user to be able to host on free public hoster the files and folders to configure an OMI project.
// That give the creator free space where to store their creation and users of OMI to just have to point at those to install the configuration.
// They can still copy past the file a folder but if the want, they can refer to a pointers register.
// All the files and folders will be copy locally where the code of the developper want it.
// The main goal is to make it works with FTP and GitHub.
// But to maintain until it is ban pointer to GAFA that allows to store file like Google.

// Will I do version that can connect to private account?
// Maybe but not in a first place as I would prefer the community around the tool to stay open source.


public interface IRemove2Files_GoogleSheet
{
    public void GetGoogleSheetAsText(in string url, out string text);
    public void CreateFile(in string absoluteFilePath, in string googleSheetUrl);
}
public interface IRemove2Files_GoogleDocument
{
    public void GetGoogleDocumentAsText(in string url, out string text);
    public void CreateFile(in string absoluteFilePath, in string googleDocUrl);
}


public interface IRemote2Files_ByGitHubCloneAndPull {

    public bool IsGitInstallOnDevice();
    public void CloneOrPull(in string absoluteDirectoryPath, in string githubLink);
}

//https://stackoverflow.com/questions/41980082/download-public-google-drive-files
//https://docs.google.com/uc?id=FILE_ID
///
public interface IRemove2Files_GoogleDriveFiles {

    public void DownloadFile(in string absoluteDirectoryPath, in string googleDriveUrl);
    public void DownloadDirectory(in string absoluteDirectoryPath, in string googleDriveDirectoryUrl);
}



////////Experimental is almost a toolboxes alone.

/// <summary>
/// Technically if you download a GithubPage, you have the html code to know where are the file and folder.
/// So you can have the url of the other files and folder to download. So with recurcivity you could download all the github without having access to the API.
/// Allowing to use Github as host. Maybe won't work with basic HTML download in C# See Selenium if it don't works.
/// </summary>
public interface IRemote2Files_DownloadPublicGitHubByUsingHTML
{
    public void Download(in string absoluteDirectoryPath, in string githubLink);
}

public interface IRemote2Files_DownloadPublicGitHubByUsingSelenium
{
    public void Download(in string absoluteDirectoryPath, in string githubLink);
}
