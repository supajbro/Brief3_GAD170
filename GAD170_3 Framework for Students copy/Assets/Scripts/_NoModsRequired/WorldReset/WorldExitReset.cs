using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detect when a physics object leaves our volume and reset it, optionally delegating that logic to the object itself.
/// 
/// See: WorldExitResponse
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
public class WorldExitReset : MonoBehaviour
{
    public Transform defaultResetPos;

    private void OnTriggerExit(Collider other)
    {
        //notify via game event
        var data = new GameEvents.ObjectResetData();
        data.offendingCollider = other;
        data.offendingResetter = this;
        data.positionPriorToReset = other.attachedRigidbody != null ? other.attachedRigidbody.transform.position : other.transform.position;

        //if it has the beh tell it to deal with it
        var resp = other.gameObject.GetComponent<WorldExitResponse>();
        if (resp != null)
        {
            resp.ResetNow(this);
        }
        else
        {
            WorldExitResponse.DefaultResponse(other,this);
        }

        GameEvents.InvokeObjectReset(data);
    }
}
