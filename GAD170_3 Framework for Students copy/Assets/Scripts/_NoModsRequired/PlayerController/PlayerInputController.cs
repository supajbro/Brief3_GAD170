using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Routes player inputs to movement and interaction
/// </summary>
[RequireComponent(typeof(SweepMover),
    typeof(PlayerItemInteraction))]
public class PlayerInputController : MonoBehaviour
{
    public string horizontalAxisName = "Horizontal";
    public string veritcalAxisName = "Vertical";
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode pickUpKey = KeyCode.E;
    public KeyCode useKey = KeyCode.Mouse0;
    public KeyCode throwKey = KeyCode.Mouse1;

    protected SweepMover sweepMover;
    protected PlayerItemInteraction playerItemInteraction;

    // Start is called before the first frame update
    void Start()
    {
        sweepMover = GetComponent<SweepMover>();
        playerItemInteraction = GetComponent<PlayerItemInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        var jumpPlease = Input.GetKeyDown(jumpKey);
        var pickup = Input.GetKeyDown(pickUpKey);
        var use = Input.GetKeyDown(useKey);
        var throwNow = Input.GetKeyDown(throwKey);

        playerItemInteraction.InteractAndUpdate(pickup, use, throwNow);

        var moveVector = new Vector3(Input.GetAxis(horizontalAxisName), 0, Input.GetAxis(veritcalAxisName));
        if (moveVector.magnitude > 1)
            moveVector.Normalize();

        sweepMover.MoveAndUpdate(moveVector, jumpPlease);
    }
}
