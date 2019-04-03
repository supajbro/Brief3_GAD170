using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item that when used while held acts as a physics based projectile instantiator
/// </summary>
public class NerfGunItem : InteractiveItem
{
    public GameObject nerfDartPrefab;
    public Transform nerfDartSpawnLocation;
    public float fireRate = 1;
    public float launchForce = 10;
    protected float fireRateCounter;

    protected void Update()
    {
    }

    public override void OnUse()
    {
        base.OnUse();

        //TODO: we need to determine if we can fire and if so, make the thing
    }

    public void FireNow()
    {
        //TODO: this is where we would actually create the thing and get it on its way
    }
}
