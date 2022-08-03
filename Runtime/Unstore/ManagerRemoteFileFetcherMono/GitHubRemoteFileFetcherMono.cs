using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitHubRemoteFileFetcherMono : AbstractRemoteFileFetcherMono
{
    public override bool CanYouHandleThePath(in string path)
    {
        return GitHubRemoteUtility.IsGitHubRelated(in path);
    }

    public override void GetFileTextFromPath(in string path, out string text)
    {
        GitHubRemoteUtility.CreateRawGitLink(in path, out string url);
        if (string.IsNullOrEmpty(url)) { 
            text = "";
            return;
        }
        ISingleStringPointerHolder pointer = new SingleStringPointerHolderStruct(url);
        FileDownloadCallback callback = new FileDownloadCallback(url);
        RemoteAccessUtility.DownloadFileAsText_CSharpClassic(pointer, callback);
        callback.GetErrorCount(out int error);
        if (error > 0)
        {
            text = "";
            return;
        }
        callback.GetTextDownloaded(out text);
    }

    

    public override void GetFileTextFromPathRootRelative(in string pathRoot, in string relativeFilePath, out string text)
    {
        throw new System.NotImplementedException();
    }
}
