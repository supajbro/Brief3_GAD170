using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For objects that don't need to reset but should be just cleaned up when they go out of bounds
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
public class WorldResetDestroy : WorldExitResponse
{
    public override void ResetNow(WorldExitReset resetBy)
    {
        Destroy(gameObject);
    }
}
