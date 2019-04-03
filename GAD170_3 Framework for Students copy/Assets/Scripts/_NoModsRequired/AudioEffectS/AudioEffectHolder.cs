using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple sfx component, allows for configuring on a game object and triggering it via unity event or other means
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
public class AudioEffectHolder : MonoBehaviour
{
    public AudioEffectSO sfx;

    public void DoSFX()
    {
        if (sfx != null)
            sfx.Play2D();
    }
}
