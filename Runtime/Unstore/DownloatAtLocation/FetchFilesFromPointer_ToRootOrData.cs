using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchFilesFromPointer_ToRootOrData : AbstractRelativeFetchFilesFromPointerMono
{


    public override string GetAbstractPath()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        return Application.dataPath+"/../";

#else
        return Application.persistentDataPath;
#endif
    }

}