using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Exp_Urlrelativefilepointers : MonoBehaviour

{

    public string m_rootwhereToCreate;
    public AbstractRemoteFileFetcherMono m_downloaded;
    [TextArea(0, 10)]
    public string m_text;


    [ContextMenu("Create folders")]
    public void Create()
    {
       // Exp_UrlrelativefilepointersUtility.CreateDirectories(
           // in m_rootwhereToCreate, in m_text);
    }
    [ContextMenu("Open Path")]
    public void OpenPath()
    {
        Application.OpenURL(m_rootwhereToCreate);
    }
}


public class Exp_UrlrelativefilepointersUtility
{

    //public static void CreateDirectories(in string absoluteDirectoryPath,in IAbstractFileFetcher fetcher, 
    //    in string textInFile)
    //{
    //    RemoteAccessStringUtility.SplitToLine(textInFile, out string[] l);
    //    CreateDirectories(in absoluteDirectoryPath, fetcher, l);
    //}
    //public static void CreateDirectories(in string absoluteDirectoryPath, in IAbstractFileFetcher fetcher,
    //   in string[] line)
    //{
    //    Directory.CreateDirectory(absoluteDirectoryPath);
    //    for (int i = 0; i < line.Length; i++)
    //    {
    //        CreateDirectoriesFromLine(in absoluteDirectoryPath, fetcher, in line[i]);
    //    }
    //}

    //private static void CreateDirectoriesFromLine(in string absoluteDirectoryPath, in IAbstractFileFetcher fetcher,
    //    in string line)
    //{
    //    SplitInDouble(in line, out string relativeToStore, out string WhereToFetch);
    //    if(fetcher.GetFileTextFromPath(in WhereToFetch))
    //}

    public static void SplitInDouble(in string line, out string relativeToStore, out string WhereToFetch) {
        int dots = line.IndexOf(":");
        int https = line.IndexOf("http");
        if (dots > https)
        {
            relativeToStore  = "";
            WhereToFetch = line;
        }
        else
        {
             relativeToStore = line.Substring(0, dots);
             WhereToFetch = dots + 1 >= line.Length ? "" : line.Substring(dots + 1);
        }
    }
}