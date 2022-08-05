using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreateRelativeFilesPointer : MonoBehaviour
{
    public CreateFilesPointerMono m_creatorMono;
    public InputField m_directoryPath;
    public InputField m_filePath;
    public InputField m_filterCantHaveSegment ;
    public InputField m_filterCantHaveEndFile ;
    public InputField m_filterCantHaveStartFile;
    public InputField m_filterCantHaveRegex;


    public void Awake()
    {
        RefreshUI();
    }

    [ContextMenu("Create pointer")]
    public  void CreatePointer()
    {
        m_creatorMono.CreatePointer();
    }

    [ContextMenu("Refresh UI")]
    public void RefreshUI()
    {
        m_creatorMono.GetFileNameNoExtension(out string filePath); m_filePath.text = filePath;
        m_creatorMono.GetDirectory(out string directoryPath); m_directoryPath.text = directoryPath;
        m_creatorMono.GetCantHaveSegment(out string filterCantHaveSegment); m_filterCantHaveSegment.text = filterCantHaveSegment;
        m_creatorMono.GetCantEndBy(out string filterCantHaveEndFile); m_filterCantHaveEndFile.text = filterCantHaveEndFile;
        m_creatorMono.GetCantStartBy(out string filterCantHaveStartFile); m_filterCantHaveStartFile.text = filterCantHaveStartFile;
        m_creatorMono.GetCantHaveRegex(out string filterCantHaveRegex); m_filterCantHaveRegex.text = filterCantHaveRegex;
    }
    [ContextMenu("Refresh UI")]
    public void UIToData()
    {
        m_creatorMono.SetFileNameNoExtension(m_filePath.text);
        m_creatorMono.SetDirectory(m_directoryPath.text);
        m_creatorMono.SetCantHaveSegment(m_filterCantHaveSegment.text);
        m_creatorMono.SetCantEndBy(m_filterCantHaveEndFile.text);
        m_creatorMono.SetCantStartBy(m_filterCantHaveStartFile.text);
        m_creatorMono.SetCantHaveRegex(m_filterCantHaveRegex.text);
    }

    public string m_saveId= "CreatePointer";

    public bool m_usePlayerPrefs=true;

    private void OnEnable()
    {
        if (m_usePlayerPrefs) { 
            Reload();
            RefreshUI();
        }
    }
    private void OnDisable()
    {
        if (m_usePlayerPrefs) { 
            Save();
            RefreshUI();
        }
    }

    public void Save() {
       string json = JsonUtility.ToJson(m_creatorMono.m_ignoreFilter);
        PlayerPrefs.SetString(m_saveId, json);
        
    }
    public void Reload() {
        if (PlayerPrefs.HasKey(m_saveId)) { 
            m_creatorMono.m_ignoreFilter= JsonUtility.FromJson<FileIgnoreFilter>( PlayerPrefs.GetString(m_saveId));
        }
    }


}
