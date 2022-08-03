using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRemoteFileFetcherMono : AbstractRemoteFileFetcherMono, IAbstractFileFetcher
{
    public AbstractRemoteFileFetcherMono[] m_fetcher;

    public override bool CanYouHandleThePath(in string path)
    {
        for (int i = 0; i < m_fetcher.Length; i++)
        {
            if (m_fetcher[i]!=null && m_fetcher[i].CanYouHandleThePath(in path))
                return true;
        }
        return false;
    }

    public override void GetFileTextFromPath(in string path, out string text)
    {
        for (int i = 0; i < m_fetcher.Length; i++)
        {
            if (m_fetcher[i] != null && m_fetcher[i].CanYouHandleThePath(in path)) {
                m_fetcher[i].GetFileTextFromPath(in path, out text);
                return;
            }
        }
        text = "";
    }

    public override void GetFileTextFromPathRootRelative(in string pathRoot, in string relativeFilePath, out string text)
    {
        for (int i = 0; i < m_fetcher.Length; i++)
        {
            if (m_fetcher[i] != null && m_fetcher[i].CanYouHandleThePath(in pathRoot))
            {
                m_fetcher[i].GetFileTextFromPathRootRelative(in pathRoot, in relativeFilePath, out text);
                return;
            }
        }
        text = "";
    }
}


public interface IAbstractFileFetcher {

    public  void GetFileTextFromPath(in string path, out string text);
    public  void GetFileTextFromPathRootRelative(in string pathRoot, in string relativeFilePath, out string text);
}
public abstract class AbstractRemoteFileFetcherMono : MonoBehaviour, IAbstractFileFetcher
{
    public abstract bool CanYouHandleThePath(in string path);
    public abstract void GetFileTextFromPath(in string path, out string text);
    public abstract void GetFileTextFromPathRootRelative(in string pathRoot, in string relativeFilePath, out string text);
}
