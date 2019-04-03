using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data object and logic for 1 audio effect, with basis customisation available.
/// 
/// NOTE: Provided with framework, no modification required
/// 
/// Future work: 
///   3d support within the Effect or handled by external user
///   support mixer groups
/// </summary>
[CreateAssetMenu()]
public class AudioEffectSO : ScriptableObject
{
    public AudioClip[] clips;
    [Range(0.0f, 1.0f)]
    public float minVol = 1, maxVol = 1;
    [Range(-3.0f, 3.0f)]
    public float minPitch = 1, maxPitch = 1;

    public void Play(AudioSource source, bool oneShot = true, float pan = 0)
    {
        if (source == null || clips == null || clips.Length == 0)
            return; //bail out not possible

        var clip = clips[Random.Range(0, clips.Length)];

        if (oneShot)
        {
            //one shot's stack on the source so we don't want to mess with existing pitch or vol
            source.PlayOneShot(clip);
        }
        else
        {
            var curVol = Random.Range(minVol, maxVol);
            var curPitch = Random.Range(minPitch, maxPitch);
            source.clip = clip;
            source.pitch = curPitch;
            source.volume = curVol;
            source.panStereo = pan;
            source.Play();
        }
    }

    public void Play2D()
    {
        var s = AudioSourcePool.GetSource();
        s.spatialBlend = 0;
        Play(s, false, 0);
        AudioSourcePool.ReturnSourceWhenDone(s);
    }

    public void Play2D(float pan = 0)
    {
        var s = AudioSourcePool.GetSource();
        s.spatialBlend = 0;
        Play(s, false, pan);
        AudioSourcePool.ReturnSourceWhenDone(s);
    }
}