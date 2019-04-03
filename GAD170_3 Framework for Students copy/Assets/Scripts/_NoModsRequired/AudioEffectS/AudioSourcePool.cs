using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Simple helper for having a collection of AudioSources for re-use.
/// Presently not optimised.
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
[AddComponentMenu("")]//do not show as addable component
public class AudioSourcePool : MonoBehaviour
{
    #region singleton stuff
    static private AudioSourcePool _inst;
    static public AudioSourcePool Inst
    {
        get
        {
            if(_inst == null)
            {
                _inst = FindObjectOfType<AudioSourcePool>();

                if (_inst == null)
                {
                    _inst = new GameObject("AudioSourcePool").AddComponent<AudioSourcePool>();
                }
            }

            return _inst;
        }
    }
    #endregion

    [SerializeField]
    private List<AudioSource> available = new List<AudioSource>();

    static public AudioSource GetSource()
    {
        if (Inst.available.Count == 0)
        {
            var newGO = new GameObject();
            newGO.transform.parent = Inst.transform;
            Inst.available.Add(newGO.AddComponent<AudioSource>());
        }

        var s = Inst.available.Last();
        Inst.available.RemoveAt(Inst.available.Count - 1);
        return s;
    }

    public static void ReturnSourceWhenDone(AudioSource s)
    {
        Inst.StartCoroutine(ReturnWhenDoneInternal(s));
    }

    static private IEnumerator ReturnWhenDoneInternal(AudioSource s)
    {
        if(s != null && s.clip != null)
            yield return new WaitForSeconds(s.clip.length);

        ReturnSource(s);
    }

    public static void ReturnSource(AudioSource s)
    {
        if(s != null)
            Inst.available.Add(s);
    }
}
