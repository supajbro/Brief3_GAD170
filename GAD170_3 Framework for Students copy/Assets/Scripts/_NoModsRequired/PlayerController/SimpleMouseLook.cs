using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Based on unity's mouse look from its standard assets package
/// 
/// NOTE: Provided with framework, no modification required
/// 
/// Future work;
///   Input smoothing
///   Persist sensitivity and invertedness
/// </summary>
public class SimpleMouseLook : MonoBehaviour
{
    public Transform yawTransform, pitchTransform;
    public float yawSpeed, pitchSpeed;
    protected Vector2 yawPitch;
    public string yawAxisName = "Mouse X";
    public string pitchAxisName = "Mouse Y";

    protected Quaternion yawTransRot, pitchTransRot;
    protected bool cursorLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        yawTransRot = yawTransform.localRotation;
        pitchTransRot = pitchTransform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        var desiredChangePitch = Input.GetAxisRaw(pitchAxisName)* Time.deltaTime * pitchSpeed;
        var desiredChangeYaw = Input.GetAxisRaw(yawAxisName) * Time.deltaTime * yawSpeed;

        yawTransRot *= Quaternion.AngleAxis(desiredChangeYaw,Vector3.up);
        yawTransform.localRotation = yawTransRot;

        pitchTransRot *= Quaternion.AngleAxis(desiredChangePitch, Vector3.left);
        pitchTransRot = ClampPitchRot(pitchTransRot);
        pitchTransform.localRotation = pitchTransRot;

        UplodateCursorLock();
    }

    private void UplodateCursorLock()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursorLocked = true;
        }

        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!cursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampPitchRot(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -85, 85);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
