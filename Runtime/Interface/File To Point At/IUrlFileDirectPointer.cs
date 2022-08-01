using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUrlFileDirectPointer : ISingleStringPointerHolder
{
    public bool HasRelativePath();
    public void GetRelativePathOFile(out string relativePathToUse);
    public void GetUrlToFetchAsFile(out string urlToFetch);
}
