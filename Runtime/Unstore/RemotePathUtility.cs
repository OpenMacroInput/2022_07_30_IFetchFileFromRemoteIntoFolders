using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RemotePathUtility
{

    public static bool UrlEndByFile(in string path) { GetFileExtensionWithDotOfUrl(in path, out string ext); return ext.Length > 0; }
    public static void GetFileExtensionWithDotOfUrl(in string path, out string extensionwithDot)
    {

        extensionwithDot = "";
        for (int i = path.Length - 1; i >= 0; i--)
        {
            if (path[i] == '/' || path[i] == '\\')
            {
                if (i == 0)
                {
                    return;
                }
                else
                {
                    extensionwithDot = path.Substring(i + 1);
                    return;
                }
            }
        }
    }
}