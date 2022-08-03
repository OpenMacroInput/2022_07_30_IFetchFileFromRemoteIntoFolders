using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Exp_DirectoriesConstructor : MonoBehaviour
{

    public string m_rootwhereToCreate;
    [TextArea(0,10)]
    public string m_text;


    [ContextMenu("Create folders")]
    public void Create() {
        DirectoriesConstructorUtility.CreateDirectories(
            in m_rootwhereToCreate, in m_text);
    }
    [ContextMenu("Open Path")]
    public void OpenPath() {
        Application.OpenURL(m_rootwhereToCreate);
    }
}


public class DirectoriesConstructorUtility {

    public static void CreateDirectories(in string abosluteDirectoryPath,
        in string textInFile)
    {
        RemoteAccessStringUtility.SplitToLine(textInFile, out string []l);
        CreateDirectories(in abosluteDirectoryPath,l);
    }
    public static void CreateDirectories(in string abosluteDirectoryPath,
       in string [] relativePath)
    {
        string p = RemoteAccessStringUtility. RemoveSlashAtEnd(abosluteDirectoryPath);
        for (int i = 0; i < relativePath.Length; i++)
        {
            string pp = p + '\\' + RemoteAccessStringUtility. RemoveSlashAtStart(relativePath[i]);
            Debug.Log(">"+pp);
            Directory.CreateDirectory(pp);
        }
    }
}
