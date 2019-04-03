using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Generic item that works with the PlayerInteraction. Is intended to be subclassed for 
/// specific OnUse behaviours. Makes assumptions about rigidbody's being dynamic, will 
/// force them kinematic and triggers when held and back to nonkinematic and nontriggers when dropped.
/// 
/// TODO:
///     Most likley no changes required, but if you want to add or remove features that are common
///         to all items, this would be the place to do it.
/// </summary>
public class InteractiveItem : MonoBehaviour
{
    public bool canPickUp = true;
    public bool canUse = true;
    public float throwForceMultiplier = 1;

    public bool isHeld = false;

    public DefaultLogMessage UseWhileHeldMessage;
    public DefaultLogMessage UseNotHeldMessage;
    public AudioEffectSO usedSFX;

    protected Rigidbody rb;
    public new Rigidbody rigidbody { get { return rb; } }
    protected Collider[] colliders;


    //this exists just so we can have a nice simple foldout in the UI, not the best solution here
    // but it is a simple one
    [System.Serializable]
    public class UsedEventsHolder
    {
        public UnityEvent OnUsedWhileHeld = new UnityEvent();
        public UnityEvent OnUsedWhileNotHeld = new UnityEvent();
    }
    public UsedEventsHolder usedEventsHolder = new UsedEventsHolder();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }

    public virtual void OnDrop()
    {
        transform.parent = null;

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].isTrigger = false;
        }

        isHeld = false;
    }

    public virtual void OnPickUp(Transform goHere)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].isTrigger = true;
        }

        transform.parent = goHere;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        isHeld = true;
    }

    public virtual void OnUse()
    {
        BattleLog.Log(isHeld ? UseWhileHeldMessage : UseNotHeldMessage);
        if (usedSFX != null)
            usedSFX.Play2D();

        if(usedEventsHolder != null)
        {
            if(isHeld)
            {
                usedEventsHolder.OnUsedWhileHeld.Invoke();
            }
            else
            {
                usedEventsHolder.OnUsedWhileNotHeld.Invoke();
            }
        }
    }
}
