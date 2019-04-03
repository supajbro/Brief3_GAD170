using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sphere cast based item pickup system. Allows for collection of a single item, as
/// definied by colliders that have InteractionItem scripts on them. The item can be
/// used, pickup, dropped and thrown.
/// 
/// Most likely needs to be on your player or camera object
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
public class PlayerItemInteraction : MonoBehaviour
{
    public Transform syncHeldItemHere;
    public Transform interactFrom;
    public LayerMask selectionLayers = ~0;
    public float interactDistance = 1.5f;
    public float selectionRadius = 0.1f;
    public float throwingImpulse = 10;

    protected RaycastHit[] hitResults = new RaycastHit[128];
    protected List<InteractiveItem> potentialItems = new List<InteractiveItem>();
    protected InteractiveItem toPickUp;
    protected InteractiveItem heldItem;

    protected void SetToPickUp(InteractiveItem item)
    {
        if (toPickUp != item)
        {
            toPickUp = item;

            GameEvents.InvokeInteractiveItemChange(new GameEvents.InteractiveItemData(item, 
                GameEvents.InteractiveItemData.InteractionType.Presented, this));
        }
    }

    public void InteractAndUpdate(bool requestPickup, bool requestUse, bool requestThrow)
    {
        if (requestPickup)
        {
            if (heldItem != null)
            {
                DropItem();
            }

            if (toPickUp != null && toPickUp.canPickUp)
            {
                PickUpitem();
            }
        }

        if (requestUse)
        {
            if (heldItem != null && heldItem.canUse)
            {
                GameEvents.InvokeInteractiveItemChange(new GameEvents.InteractiveItemData(heldItem,
                    GameEvents.InteractiveItemData.InteractionType.UsedWhileHeld, this));
                heldItem.OnUse();
            }
            else if (toPickUp != null && toPickUp.canUse)
            {
                GameEvents.InvokeInteractiveItemChange(new GameEvents.InteractiveItemData(toPickUp,
                    GameEvents.InteractiveItemData.InteractionType.Used, this));
                toPickUp.OnUse();
            }
        }

        if (requestThrow)
        {
            //only right mouse throw, 'throwable' objects
            if (heldItem != null && heldItem.rigidbody != null)
            {
                var tmp = heldItem;
                DropItem();
                //apply impulse
                tmp.rigidbody.AddForce(interactFrom.forward * throwingImpulse * tmp.throwForceMultiplier, ForceMode.Impulse);
                GameEvents.InvokeInteractiveItemChange(new GameEvents.InteractiveItemData(tmp,
                    GameEvents.InteractiveItemData.InteractionType.Thrown, this));
            }
        }

        GatherPotentials();
    }

    protected void GatherPotentials()
    {
        potentialItems.Clear();

        var numHit = Physics.SphereCastNonAlloc(interactFrom.position, selectionRadius, interactFrom.forward,
            hitResults, interactDistance, selectionLayers.value, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < numHit; i++)
        {
            var comp = hitResults[i].collider.gameObject.GetComponent<InteractiveItem>();

            if(comp == null)
            {
                //ok try its rb go in case that is different
                comp = hitResults[i].collider.attachedRigidbody.GetComponent<InteractiveItem>();
            }

            if (comp != null && !comp.isHeld)
            {
                potentialItems.Add(comp);
            }
        }

        //sort em

        if (potentialItems.Count != 0)
        {
            SetToPickUp(potentialItems[0]);
        }
        else
        {
            SetToPickUp(null);
        }
    }

    protected void DropItem()
    {
        heldItem.OnDrop();
        var tmp = heldItem;
        heldItem = null;

        GameEvents.InvokeInteractiveItemChange(new GameEvents.InteractiveItemData(tmp,
            GameEvents.InteractiveItemData.InteractionType.Dropped, this));
    }

    protected void PickUpitem()
    {
        heldItem = toPickUp;
        heldItem.OnPickUp(syncHeldItemHere);
        GameEvents.InvokeInteractiveItemChange(new GameEvents.InteractiveItemData(heldItem,
            GameEvents.InteractiveItemData.InteractionType.PickedUp, this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (interactFrom != null)
        {
            Gizmos.DrawWireSphere(interactFrom.position + interactFrom.forward * interactDistance, selectionRadius);
            Gizmos.DrawRay(interactFrom.position, interactFrom.forward * interactDistance);
        }
    }
}
