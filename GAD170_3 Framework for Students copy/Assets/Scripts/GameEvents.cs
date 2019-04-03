using UnityEngine;

/// <summary>
/// Central routing of global events in our game. These are c# style delegate events. You need 
/// to be sure that you are += to sub and -= to unsub. You always need a matching unsub, without 
/// it you may be causing a form of memory leak. For a component, this is generally in OnEnable 
/// and OnDisable
/// 
/// This method requires a bit of boiler plate so it looks more intimidating that it actually is.
/// First you need to determine the data bundle, this is a class or struct that contains all 
///     the information your event intends to send, by using this bundle technique it is easy
///     to add more elements if they are required later. Same idea behind Unity's Collision
/// Second you define the delegate, this is almost always void _NAME_Del (_DATA_BUNDLE data).
///     A function that has no return and takes a single param of the data bundle type.
/// Third you need to define the events that use that delegate and data bundle.
///     Typically only 1, it will almost always be event _NAME_Del On_NAME, followed by
///     Invoke_NAME_(_DATA_BUNDLE data)
///         if(On_NAME_ != null)
///             On_NAME_(data)
/// 
/// Future Work;
///     This setup is quickly outstaying its welcome. A more ideal solution for our game events
///         does not require them to be hard wired to a specific static variable and would be
///         configurable/viewable in the inspector or some custom window, see things like 
///         Game Archietecture with Scriptable Objects, Unity Atoms, nad/or Event Dispatch as 
///         a potential way forward.
/// </summary>
public static class GameEvents
{
    #region data bundle declares
    [System.Serializable]
    public class ObjectResetData
    {
        public Vector3 positionPriorToReset;
        public Collider offendingCollider;
        public WorldExitReset offendingResetter;
    }

    [System.Serializable]
    public class CheckListItemChangedData
    {
        public float previousItemProgress;
        public CheckListItem item;
    }

    [System.Serializable]
    public class InteractiveItemData
    {
        public InteractiveItem item;
        public enum InteractionType
        {
            Presented,
            PickedUp,
            Dropped,
            UsedWhileHeld,
            Used,
            Thrown,
        }

        public InteractionType type;
        public PlayerItemInteraction interactionController;

        public InteractiveItemData(InteractiveItem interactiveItem, InteractionType interactionType, PlayerItemInteraction playerItemInteraction)
        {
            item = interactiveItem;
            type = interactionType;
            interactionController = playerItemInteraction;
        }
    }

    [System.Serializable]
    public class PhysicsInteractionData
    {
        public GameObject instigator, other;
    }
    #endregion

    #region delegate declares
    public delegate void ObjectResetDel(ObjectResetData data);
    public delegate void CheckListItemChangedDel(CheckListItemChangedData data);
    public delegate void InteractiveItemDel(InteractiveItemData data);
    public delegate void PhysicsInteractionDel(PhysicsInteractionData data);
    #endregion

    public static event ObjectResetDel OnObjectReset;
    public static void InvokeObjectReset(ObjectResetData data)
    {
        if (OnObjectReset != null)
            OnObjectReset(data);
    }

    public static event CheckListItemChangedDel OnCheckListItemChanged;
    public static void InvokeCheckListItemChanged(CheckListItemChangedData data)
    {
        if (OnCheckListItemChanged != null)
            OnCheckListItemChanged(data);
    }

    public static event InteractiveItemDel OnIteractiveItemChange;
    public static void InvokeInteractiveItemChange(InteractiveItemData data)
    {
        if (OnIteractiveItemChange != null)
            OnIteractiveItemChange(data);
    }

    public static event PhysicsInteractionDel OnImportantPhysicsCollision;
    public static void InvokeImportantPhysicsCollision(PhysicsInteractionData data)
    {
        if (OnImportantPhysicsCollision != null)
            OnImportantPhysicsCollision(data);
    }
}
