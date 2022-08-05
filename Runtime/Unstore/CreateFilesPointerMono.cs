using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class CreateFilesPointerMono : MonoBehaviour
{
    public string m_directoryPath;
    public string m_defaultFileName;
    public FilesPointerBuildLog m_buildLog;
    public FileIgnoreFilter m_ignoreFilter;
    [ContextMenu("Refresh")]
    public void Refresh() {
        FilesPointerBuilderUtility.Create(in m_directoryPath, m_defaultFileName, in m_ignoreFilter, out m_buildLog);
    }

    [ContextMenu("Set Load and switch to slash")]
    public void ToLowerAndSlash()
    {
        m_ignoreFilter.SetArrayToLowerAndReplaceBackSlash();
       
    }

    
}
public class FilesPointerBuilderUtility {

    public static void ReplaceSlashAndPutLow(ref string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = array[i].ToLower().Replace("\\", "/");
        }
    }
    public static string ReplaceSlashAndPutLow(in string path)
    {
        return path.ToLower().Replace("\\", "/");

    }
    public static void Create(in string rootDirectory, in string fileNameNoExtension, out FilesPointerBuildLog builder)
    {

        string pointerPath = RemoteAccessStringUtility.RemoveSlashAtEnd(in rootDirectory) + "/" + fileNameNoExtension + ".relativefilespointer";
        builder = new FilesPointerBuildLog();

        if (!Directory.Exists(rootDirectory))
            throw new System.Exception();
        File.WriteAllText(pointerPath, "");

        builder.m_rootDirectory = rootDirectory;
        builder.m_fileName = fileNameNoExtension;
        builder.m_absoluteFiles = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);
        builder.m_absoluteDirectory = Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories);
    
        builder.m_relativeFiles = new string[builder.m_absoluteFiles.Length];
        builder.m_relativeDirectory = new string[builder.m_absoluteDirectory.Length];
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < builder.m_absoluteFiles.Length; i++)
        {
            builder.m_relativeFiles[i] = RemoteAccessStringUtility.RemoveSlashAtStart(builder.m_absoluteFiles[i].Replace(rootDirectory, ""));
            sb.AppendLine(builder.m_relativeFiles[i]);
        }
        for (int i = 0; i < builder.m_absoluteDirectory.Length; i++)
        {
            builder.m_relativeDirectory[i] = RemoteAccessStringUtility.RemoveSlashAtStart(builder.m_absoluteDirectory[i].Replace(rootDirectory, ""));
            sb.AppendLine(builder.m_relativeDirectory[i]);
        }

        builder.m_resultPathToCreate = pointerPath;
        builder.m_resultFileText = sb.ToString();
        File.WriteAllText(builder.m_resultPathToCreate, builder.m_resultFileText);
    }


    public static void Create(in string rootDirectory, in string fileNameNoExtension, in FileIgnoreFilter ignoreFilter, out FilesPointerBuildLog builder)
    {

        string pointerPath = RemoteAccessStringUtility.RemoveSlashAtEnd(in rootDirectory) + "/" + fileNameNoExtension + ".relativefilespointer";
        builder = new FilesPointerBuildLog();

        if (!Directory.Exists(rootDirectory))
            throw new System.Exception();
        File.WriteAllText(pointerPath, "");

        builder.m_rootDirectory = rootDirectory;
        builder.m_fileName = fileNameNoExtension;
        builder.m_absoluteFiles = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);
        builder.m_absoluteDirectory = Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories);
        builder.m_absoluteFiles = RemoveUnwantedPath(in builder.m_absoluteFiles, ignoreFilter);
        builder.m_absoluteDirectory = RemoveUnwantedPath(in builder.m_absoluteDirectory, ignoreFilter);

        builder.m_relativeFiles = new string[builder.m_absoluteFiles.Length];
        builder.m_relativeDirectory = new string[builder.m_absoluteDirectory.Length];
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < builder.m_absoluteFiles.Length; i++)
        {
            builder.m_relativeFiles[i] = RemoteAccessStringUtility.RemoveSlashAtStart(builder.m_absoluteFiles[i].Replace(rootDirectory, ""));
            sb.AppendLine(builder.m_relativeFiles[i]);
        }
        for (int i = 0; i < builder.m_absoluteDirectory.Length; i++)
        {
            builder.m_relativeDirectory[i] = RemoteAccessStringUtility.RemoveSlashAtStart(builder.m_absoluteDirectory[i].Replace(rootDirectory, ""));
            sb.AppendLine(builder.m_relativeDirectory[i]);
        }

        builder.m_resultPathToCreate = pointerPath;
        builder.m_resultFileText = sb.ToString();
        File.WriteAllText(builder.m_resultPathToCreate, builder.m_resultFileText);
    }




    private static string[] RemoveUnwantedPath(in string[] paths,in FileIgnoreFilter filter)
    {
        List<string> list = paths.ToList();
        List<string> pathsLowSlash = paths.Select(k=> FilesPointerBuilderUtility.ReplaceSlashAndPutLow(k)).ToList();
        List<int> indexRemove = new List<int>();
        for (int i = 0; i < pathsLowSlash.Count; i++)
        {

            // If Containt Segment add Remove
            for (int j = 0; j < filter.m_cantHaveSegment.Length; j++)
            {
                if (pathsLowSlash[i].IndexOf(filter.m_cantHaveSegment[j])>-1)
                {
                    indexRemove.Add(i);
                    continue;
                }
            }
            for (int j = 0; j < filter.m_cantHaveEndFile.Length; j++)
            {
                if (RemoteAccessStringUtility.EndWith(pathsLowSlash[i],filter.m_cantHaveEndFile[j]))
                {
                    indexRemove.Add(i);
                    continue;
                }
            }
            for (int j = 0; j < filter.m_cantHaveStartFile.Length; j++)
            {
                if (RemoteAccessStringUtility.StarWith(pathsLowSlash[i], filter.m_cantHaveStartFile[j]))
                {
                    indexRemove.Add(i);
                    continue;
                }
            }
            for (int j = 0; j < filter.m_cantHaveStartFile.Length; j++)
            {
                Regex r = new Regex(filter.m_cantHaveRegex[j]);
                if (r.IsMatch(list[i]))
                {
                    indexRemove.Add(i);
                    continue;
                }
            }
        }
        foreach (int i in indexRemove.OrderByDescending(k => k))
        {
            list.RemoveAt(i);
        }
        return list.ToArray();
    }
}


[System.Serializable]
public class FilesPointerBuildLog
{
    public string m_fileName;
    public string m_rootDirectory;
    public string[] m_absoluteFiles;
    public string[] m_relativeFiles;
    public string m_resultFileText;
    public string m_resultPathToCreate;


    public string[] m_absoluteDirectory;
    public string[] m_relativeDirectory;

}

[System.Serializable]
public class FileIgnoreFilter {

    public string[] m_cantHaveSegment = new string[] { "/.git/" };
    public string[] m_cantHaveEndFile = new string[] { ".asmdef" };
    public string[] m_cantHaveStartFile = new string[] { "/Build/" };
    public string[] m_cantHaveRegex = new string[] { ".[Jj][Pp][Ee]?[Gg]" };

    internal void SetArrayToLowerAndReplaceBackSlash()
    {
        FilesPointerBuilderUtility.ReplaceSlashAndPutLow(ref m_cantHaveSegment);
        FilesPointerBuilderUtility.ReplaceSlashAndPutLow(ref m_cantHaveEndFile);
        FilesPointerBuilderUtility.ReplaceSlashAndPutLow(ref m_cantHaveStartFile);
    }
}