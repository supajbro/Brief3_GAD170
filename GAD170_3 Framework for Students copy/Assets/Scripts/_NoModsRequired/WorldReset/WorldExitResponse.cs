using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for custom per object actions to be taken when the object is asked to reset by the WorldReseter
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
abstract public class WorldExitResponse : MonoBehaviour
{
    abstract public void ResetNow(WorldExitReset resetBy);

    public static void DefaultResponse(Collider other, WorldExitReset wer)
    {
        if (other.attachedRigidbody != null)
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
            other.attachedRigidbody.transform.position = wer.defaultResetPos.position;
            other.attachedRigidbody.MovePosition( wer.defaultResetPos.position);
        }
    }
}
