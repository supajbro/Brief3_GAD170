using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Invokes a simple UnityEvent when an certain physics requirements are met, such as staying in a trigger for a certain amount of time.
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
public class PhysicsRouter : MonoBehaviour
{
    public enum MessageType
    {
        Enter,
        Exit,
        Stay
    }

    public enum InterationType
    {
        Trigger,
        Collision,
    }

    public MessageType triggerType = MessageType.Enter;
    public InterationType interationType = InterationType.Trigger;
    public float waitTimer;
    protected float waitTimerCounter;
    protected int isCounting = 0;
    public TagCollection tagsThatPass = new TagCollection();
    public UnityEvent OnConditionsMet;

    private void Update()
    {
        Tick();
    }

    protected void Tick()
    {
        if (isCounting > 0)
        {
            waitTimerCounter += Time.deltaTime;
            if (waitTimerCounter >= waitTimer)
            {
                DoThing();
            }
        }
    }

    protected void Process(MessageType cameFrom, InterationType it, GameObject go)
    {
        if (it != interationType)
            return;

        //stays are their own thing
        if (cameFrom == MessageType.Enter && triggerType == MessageType.Stay && isCounting <= 0)
        {
            if (tagsThatPass.CheckAll(go.gameObject))
            {
                waitTimerCounter = 0;
                isCounting++;
                Tick(); //force a tick now to avoid 1 frame delay bug
            }
        }

        if (cameFrom == MessageType.Exit && triggerType == MessageType.Stay && isCounting > 0)
        {
            isCounting--;
            waitTimerCounter = 0;
        }

        //instants are much simplier
        if (triggerType == cameFrom && tagsThatPass.CheckAll(go.gameObject))
        {
            DoThing();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Process(MessageType.Enter, InterationType.Trigger, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Process(MessageType.Exit, InterationType.Trigger, other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Process(MessageType.Enter, InterationType.Collision, collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        Process(MessageType.Exit, InterationType.Collision, collision.gameObject);
    }

    public void DoThing()
    {
        waitTimerCounter = 0;
        OnConditionsMet.Invoke();
    }
}