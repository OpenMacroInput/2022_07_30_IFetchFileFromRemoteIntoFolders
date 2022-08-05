using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FetchFilesFromPointer_ToRootOrData : AbstractRelativeFetchFilesFromPointerMono
{


    public override string GetAbstractPath()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        return Application.dataPath+"/../";
#elif UNITY_ANDROID
        return "/storage/emulated/0/";
#else
        return Application.persistentDataPath;
#endif
    }

    protected override void DownloadNotification()
    {
    }
}