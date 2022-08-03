using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalFileRemoteFileFetcherMono : AbstractRemoteFileFetcherMono
{
    public override bool CanYouHandleThePath(in string path)
    {
        return File.Exists(path);
    }

    public override void GetFileTextFromPath(in string path, out string text)
    {
        text = File.ReadAllText(path);
    }

    public override void GetFileTextFromPathRootRelative(in string pathRoot, in string relativeFilePath, out string text)
    {
        RemoteAccessStringUtility.Merge(in pathRoot, in relativeFilePath, out string fullPath);
        GetFileTextFromPath(in fullPath, out text);
    }

}
