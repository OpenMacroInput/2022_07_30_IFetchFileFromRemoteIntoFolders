using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IUrlRelativeFilePointers: ISingleStringPointerHolder
{

}
public interface IUrlRelativeFilePointerItem
{
    public bool HasRelativePath();
    public void GetRelativePath(out string relativePath);
    public void GetUrlToFetch(out string url);
}
