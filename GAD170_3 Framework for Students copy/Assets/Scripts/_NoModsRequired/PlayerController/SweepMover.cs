using UnityEngine;

/// <summary>
/// Capsule Sweep based movement controller. Uses the physics world to move through but
/// does not interact directly with it. It is a kinematic player controller.
/// 
/// Presently incomplete, would most likely require a number of features to be added for 
/// it to be the go to FPS Controller in your game. It is provided here so that can happen
/// should you wish to and also as an example.
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
[RequireComponent(
    typeof(Rigidbody),
    typeof(CapsuleCollider))]
public class SweepMover : MonoBehaviour
{
    public bool debugDraw = false;
    public float speed = 6;
    public float jumpSpeed = 6;
    public float jumpGraceTime = 0.1f;
    public float skinWidth = 0.05f;
    public int depenAttempts = 10;
    public int sweepMoveAttempts = 5;
    public float maxSingleSweepDistance = 0.1f;
    public float clampToSurfaceDist = 0.1f;
    public float stepHeight = 0.5f;
    //public float slopeLimit = 0;
    //public bool useVelocity = false;
    public LayerMask collisionLayers;
    public bool projectReflectedMoveOntoSurface = true;
    public bool maintainDistanceDuringReproject = true;

    public Vector3 CapsuleCentreBottom { get { return CurrentPosition + capsule.center - ((capsule.height * 0.5f) - capsule.radius) * transform.up; } }
    public Vector3 CapsuleTipBottom { get { return CurrentPosition + capsule.center - (capsule.height * 0.5f) * transform.up; } }
    public Vector3 CapsuleCentreTop { get { return CapsuleCentreBottom + transform.up * (capsule.height - capsule.radius * 2); } }
    public Vector3 CalcDirectionOfTravel { get { return (previousPosition - previousPreviousPosition).normalized; } }
    public bool CanJump { get { return IsGrounded || timeSinceGrounded < jumpGraceTime; } }
    private Vector3 lastGroundContactPoint;

    protected Vector3 jumpFallMotion;
    protected Vector3 previousPosition, previousPreviousPosition;
    protected Vector3 surfaceNormal = Vector3.up;
    protected Rigidbody rb;
    protected CapsuleCollider capsule;
    protected Vector3 desiredMove, desiredStep;
    [SerializeField] protected bool _isGrounded = false;
    protected float timeSinceGrounded;
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            timeSinceGrounded = 0;
            _isGrounded = value;
        }
    }



    public Vector3 CurrentPosition
    {
        get { return transform.position; }
    }

    private void UpdatePosition(Vector3 pos)
    {
        UpdatePosition(pos, Color.blue);
    }

    private void UpdatePosition(Vector3 pos, Color col)
    {
        if (debugDraw)
        {
            Debug.DrawLine(transform.position, pos, col, 0.2f);
        }
        transform.position = pos;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        previousPosition = CurrentPosition;
        previousPreviousPosition = previousPosition;
    }

    public void MoveAndUpdate(Vector3 inputVector, bool jumpRequested)
    {
        //var curPos = CurrentPosition;
        //should only be required if the game has moved things into us, but a worth while step none the less
        DepenetrateWorld();


        desiredMove = inputVector * speed * Time.deltaTime;

        //rotate to face local forward, would also be a good idea to align to surface normal
        // or perp to gravity
        desiredMove = transform.TransformVector(desiredMove);

        if(jumpRequested && CanJump)
        {
            jumpFallMotion += jumpSpeed * -Physics.gravity.normalized;
            IsGrounded = false;
            timeSinceGrounded = jumpGraceTime + 1;
        }

        if (!IsGrounded)
        {
            jumpFallMotion += Physics.gravity * Time.deltaTime;
            timeSinceGrounded += Time.deltaTime;
            UpdatePosition(CurrentPosition + jumpFallMotion * Time.deltaTime);
        }


        SweepStepper();


        FallingAndGroundedness();

        //if (useVelocity)
        //{
        //    var posDif = CurrentPosition - curPos;
        //    var predictedPos = CurrentPosition + rb.velocity * Time.deltaTime;
        //    transform.position = curPos;

        //    if (Time.deltaTime > 0)
        //        rb.velocity = rb.velocity + posDif * (1.0f / Time.deltaTime);
        //}

        DepenetrateWorld();

        previousPreviousPosition = previousPosition;
        previousPosition = CurrentPosition;

    }

    public void SweepStepper()
    {
        int reqiredSteps = (int)(desiredMove.magnitude / maxSingleSweepDistance);
        //TODO should know when direction changes not ask for the same dir every time, reproject hides this but it just shouldn't be what we do
        for (int i = 0; i < reqiredSteps; i++)
        {
            desiredStep = desiredMove.normalized * maxSingleSweepDistance;
            SweepStep();
        }
        desiredStep = desiredMove.normalized * (desiredMove.magnitude - (reqiredSteps * maxSingleSweepDistance));
        SweepStep();
    }

    public void SweepStep()
    {
        int bailCount = sweepMoveAttempts;
        while (desiredStep.magnitude > 0.001f && bailCount > 0)
        {
            bailCount--;
            SweepMove();
        }
    }

    public void FallingAndGroundedness()
    {
        //not falling in direction of gravity
        if (!IsGrounded && Vector3.Dot(CalcDirectionOfTravel, Physics.gravity) < 0)
            return;


        //extra + radius to avoid starting the cast already inside objects in our direction of cast
        var backDist = stepHeight + skinWidth;
        var groundedStartPoint = CapsuleCentreBottom + transform.up * backDist;
        var groundCheckDir = Physics.gravity.normalized;
        var distanceCheck = backDist + clampToSurfaceDist; //since it is distance to sphere hit we already include the radius part
        RaycastHit downHit = new RaycastHit();
        Debug.DrawRay(groundedStartPoint, groundCheckDir * distanceCheck, Color.black);
        var hitSomething = Physics.SphereCast(groundedStartPoint, capsule.radius, groundCheckDir, out downHit, distanceCheck, collisionLayers.value, QueryTriggerInteraction.Ignore);
        if (hitSomething)
        {
            surfaceNormal = downHit.normal;

            lastGroundContactPoint = groundedStartPoint + groundCheckDir * (downHit.distance + capsule.radius);
            var safeGroundPos = lastGroundContactPoint - groundCheckDir * skinWidth;
            var groundDiff = safeGroundPos - CapsuleTipBottom;

            UpdatePosition(CurrentPosition + groundDiff, Color.red);

            if (!IsGrounded)
            {
                IsGrounded = true;
                jumpFallMotion = Vector3.zero;
            }
        }
        else //no hit or too far go into falling if we need to
        {
            surfaceNormal = Physics.gravity.normalized * -1;
            if (IsGrounded)
            {
                var prevVel = previousPreviousPosition - previousPosition;
                var curVel = previousPosition - CurrentPosition;
                var curVelInDir = Vector3.Project(curVel, groundCheckDir) + Vector3.Project(prevVel, groundCheckDir);
                jumpFallMotion += curVelInDir / 2;
                IsGrounded = false;
            }
        }
    }

    /// <summary>
    /// Move our capsule out of any physics objects in our layers that have moved into us
    /// </summary>
    public void DepenetrateWorld()
    {
        var bot = CapsuleCentreBottom;
        var top = CapsuleCentreTop;
        Collider[] collidersOverlapped = null;
        for (int i = 0; i < depenAttempts; i++)
        {
            bool didDepen = false;
            collidersOverlapped = Physics.OverlapCapsule(bot, top, capsule.radius + skinWidth, collisionLayers.value, QueryTriggerInteraction.Ignore);
            for (int j = 0; j < collidersOverlapped.Length; j++)
            {
                if (collidersOverlapped[j] == capsule)
                    continue;

                if (Physics.ComputePenetration(capsule, CurrentPosition, transform.rotation,
                    collidersOverlapped[j], collidersOverlapped[j].transform.position, collidersOverlapped[j].transform.rotation,
                    out Vector3 outDir, out float outDist))
                {
                    UpdatePosition(CurrentPosition + outDir * (outDist + skinWidth), Color.yellow);
                    didDepen = true;
                }
            }

            if (!didDepen)
                break;
        }
    }

    private void SweepMove()
    {
        RaycastHit closest = new RaycastHit();
        var bot = CapsuleCentreBottom;
        var res = Physics.CapsuleCastAll(bot, CapsuleCentreTop, capsule.radius, desiredStep, desiredStep.magnitude + skinWidth, collisionLayers.value, QueryTriggerInteraction.Ignore);

        if (res.Length > 0)
        {
            var desDist = desiredStep.magnitude;
            var desDir = desiredStep.normalized;

            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].collider != capsule /*&& Vector3.Dot( res[i].normal, desDir) < -0.001f*/)
                {
                    if (closest.collider == null)
                    {
                        closest = res[i];
                    }
                    else if (closest.distance > res[i].distance)
                    {
                        closest = res[i];
                    }
                }
            }

            if (closest.collider != null)
            {
                var stepDist = closest.distance - skinWidth;
                var step = desDir * stepDist;
                UpdatePosition(CurrentPosition + step, Color.green);


                if (debugDraw)
                {
                    //These draw rays let you visualise the interaction between the movement, the surface, the ground and how we wish to slide along it
                    Debug.DrawRay(transform.position, desiredStep.normalized, Color.blue, 0.1f);
                    Debug.DrawRay(CapsuleTipBottom, surfaceNormal, Color.gray, 0.1f);
                    Debug.DrawRay(closest.point, closest.normal, Color.white, 0.1f);
                }
                //desiredMove = (Quaternion.FromToRotation(surfaceNormal, closest.normal) * desDir) * (desDist - closest.distance + skinWidth);
                desiredStep = Vector3.Reflect(desDir, closest.normal) * (desDist - stepDist);
                var curLen = desiredStep.magnitude;
                if (debugDraw)
                {
                    Debug.DrawRay(transform.position, desiredStep.normalized, Color.cyan, 0.1f);
                }
                if (projectReflectedMoveOntoSurface)
                {
                    desiredStep = Vector3.ProjectOnPlane(desiredStep, closest.normal);
                    if (maintainDistanceDuringReproject)
                    {
                        desiredStep = desiredStep.normalized * curLen;
                    }
                }
                if (debugDraw)
                {
                    Debug.DrawRay(transform.position, desiredStep.normalized, Color.white, 0.1f);
                }


                surfaceNormal = closest.normal;
            }
        }

        if (closest.collider == null)
        {
            UpdatePosition(CurrentPosition + desiredStep);
            desiredStep = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(lastGroundContactPoint, Vector3.one * 0.1f);
    }
}
