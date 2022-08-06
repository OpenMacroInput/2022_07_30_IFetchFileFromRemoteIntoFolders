using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RemoteAccessStringUtility
{

    public static string RemoveSlashAtStart(in string text)
    {
        if (text.Length > 0 && (text[0] == '/' || text[0] == '\\'))
            return text.Substring(1);
        else return text;
    }
    public static string RemoveSlashAtEnd(in string text)
    {
        if (text.Length > 0 && (text[text.Length - 1] == '/' || text[text.Length - 1] == '\\'))
            return text.Substring(0, text.Length - 1);
        else return text;
    }
    public static void SplitToLine(in string text, out string[] lines)
    {

        lines = text.Replace("\r", "").Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Trim();
        }
    }

    public static void Merge(in string pathRoot, in string relativeFilePath, out string fullPath)
    {
        fullPath = RemoveSlashAtEnd(in pathRoot) + "\\" + RemoveSlashAtStart(in relativeFilePath);
    }

    public static bool IsFilePath(in string path)
    {
       

        int indexLastSlash =path.LastIndexOf('\\');
        if(indexLastSlash<0)
            indexLastSlash = path.LastIndexOf('/');
        int indexLastDot = path.LastIndexOf('.');

        if (indexLastDot < 0)
            return false;
        if(indexLastDot<indexLastSlash)
            return false;
        return true;
    }

    public static bool IsDirectoryPath(in string path)
    {
        return !IsFilePath(in path);

    }

    public static bool EndWith(string text, string endWith)
    {
        return text.IndexOf(endWith) == text.Length - endWith.Length;
    }

    public static bool StarWith(string text, string startWith)
    {
        return text.IndexOf(startWith)==0;
    }

    public static void GetLastSlashSegmentOfPath(in string path, out string extension)
    {
        int slahsIndex = path.LastIndexOf("\\");
        if (slahsIndex < 0)
        {
            slahsIndex = path.LastIndexOf("/"); 
            
        }
        if(slahsIndex < 0)
        {
            extension = ""; ; return;
        }
        if (slahsIndex==path.Length-1){ extension = "";  return; }

        extension = path.Substring(slahsIndex+1);
    }

    public static string MergePathWithSlash(string startPaht, string toAdd)
    {
        return RemoveSlashAtEnd(startPaht) + "/" + RemoveSlashAtStart(toAdd);
    }
    public static string ReplaceWithAllSlash(string text) { return text.Replace("\\", "/"); }
    public static string ReplaceWithAllBackSlash(string text) { return text.Replace("/", "\\"); }
}
