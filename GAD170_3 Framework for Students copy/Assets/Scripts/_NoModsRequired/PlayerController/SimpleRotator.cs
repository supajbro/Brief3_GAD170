using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input based rotator, exists to allow simple rotation of player with axis
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
public class SimpleRotator : MonoBehaviour
{
    public string rotateAxisName = "Horizontal";
    [Tooltip("For -1 to 1 axis this is degrees per second")]
    public float inputScale = 100;
    public Vector3 rotateAxis = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAxis, Mathf.Deg2Rad * inputScale * Input.GetAxis(rotateAxisName), Space.Self);
    }
}
