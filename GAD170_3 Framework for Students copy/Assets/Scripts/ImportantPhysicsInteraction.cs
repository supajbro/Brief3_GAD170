using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General game event for notification of physics collision between two important objects.
/// 
/// Future work;
///   Could this be merged with PhysicsRouter
/// </summary>
public class ImportantPhysicsInteraction : MonoBehaviour
{
    public TagCollection tagsThatPass;

    private void OnCollisionEnter(Collision collision)
    {
        if(tagsThatPass.CheckAll(collision.gameObject))
        {
            var data = new GameEvents.PhysicsInteractionData();
            data.instigator = gameObject;
            data.other = collision.gameObject;
            GameEvents.InvokeImportantPhysicsCollision(data);
        }
    }
}
