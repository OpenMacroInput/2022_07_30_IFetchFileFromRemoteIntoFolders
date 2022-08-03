using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRemoteFileFetcherMono : AbstractRemoteFileFetcherMono
{
    public override bool CanYouHandleThePath(in string path)
    {
        return true;
    }

    public override void GetFileTextFromPath(in string path, out string text)
    {
        if (string.IsNullOrEmpty(path))
        {
            text = "";
            return;
        }
        ISingleStringPointerHolder pointer = new SingleStringPointerHolderStruct(path);
        FileDownloadCallback callback = new FileDownloadCallback(path);
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
        RemoteAccessStringUtility.Merge(in pathRoot, in relativeFilePath, out string fullPath);
        GetFileTextFromPath(in fullPath, out text);
    }

}
