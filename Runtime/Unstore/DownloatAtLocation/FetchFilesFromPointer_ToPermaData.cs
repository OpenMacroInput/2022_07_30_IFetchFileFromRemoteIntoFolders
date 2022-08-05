using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchFilesFromPointer_ToPermaData : AbstractRelativeFetchFilesFromPointerMono
{
    public override string GetAbstractPath()
    {
        return Application.persistentDataPath;
    }
}
