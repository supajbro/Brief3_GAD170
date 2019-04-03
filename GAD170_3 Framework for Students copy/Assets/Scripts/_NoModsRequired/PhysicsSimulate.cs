using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disable auto simulate in the physics settings and add this to the scene to run physics every frame, rather
/// than at fixed rate. Results in smoother physics movements but makes the simulation unreliable and non 
/// deterministic.
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
[DefaultExecutionOrder(1000)]
public class PhysicsSimulate : MonoBehaviour
{
    void Update()
    {
        if(Time.deltaTime > 0)
            Physics.Simulate(Time.deltaTime);
    }
}
