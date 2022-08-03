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
}
