using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//By default .directoriesconstructor
public interface IDirectoriesConstructorFile
{
    public void GetRelativeDirectoryPathPointers(out string[] relativePaths);
    public void GetRelativeDirectoryPathCount(out bool count);
}