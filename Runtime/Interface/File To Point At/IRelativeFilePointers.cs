using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRelativeFilePointers 
{
    public void GetItems(out IRelativeFilePointerItem [] items);
}
public interface IRelativeFilePointerItem {

    public bool HasLocalRelativePath();
    public void GetHowToStoreLocallyRelativePath(out string relativePathLocal);
    public void GetHowItIsStoreInRemoteRelativePath(out string relativePathRemote);
}
