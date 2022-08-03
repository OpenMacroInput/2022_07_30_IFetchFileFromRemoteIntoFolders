using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CreateFilesPointerMono : MonoBehaviour
{
    public string m_directoryPath;
    public string m_defaultFileName;
    public FilesPointerBuildLog m_buildLog;
    public bool m_ignoreGit=true;

    [ContextMenu("Refresh")]
    public void Refresh() {
        FilesPointerBuilderUtility.Create(in m_directoryPath, m_defaultFileName, out m_buildLog, in m_ignoreGit);
    }
}

public class FilesPointerBuilderUtility {


    public static void Create(in string rootDirectory, in string fileNameNoExtension, out FilesPointerBuildLog builder , in bool ignoreGit) {

        string pointerPath = RemoteAccessStringUtility.RemoveSlashAtEnd(in rootDirectory) + "/" + fileNameNoExtension + ".relativefilespointer";
        builder = new FilesPointerBuildLog();

        if (!Directory.Exists(rootDirectory))
            throw new System.Exception();
        File.WriteAllText(pointerPath, "");

        builder.m_rootDirectory = rootDirectory;
        builder.m_fileName = fileNameNoExtension;
        builder.m_absoluteFiles= Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);
        if (ignoreGit) { 
            builder.m_absoluteFiles=
                builder.m_absoluteFiles.Where(
                    k => ! ( k.IndexOf("\\.git\\") > -1 
                    || k.IndexOf("/.git/")  > -1))
                .ToArray();
        }
        builder.m_relativeFiles = new string[builder.m_absoluteFiles.Length];
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < builder.m_absoluteFiles.Length; i++)
        {
            builder.m_relativeFiles[i] = RemoteAccessStringUtility.RemoveSlashAtStart(builder.m_absoluteFiles[i].Replace(rootDirectory, ""));
            sb.AppendLine(builder.m_relativeFiles[i]);
        }

        builder.m_resultPathToCreate = pointerPath;
        builder.m_resultFileText = sb.ToString();
        File.WriteAllText( builder.m_resultPathToCreate ,  builder.m_resultFileText);
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

}