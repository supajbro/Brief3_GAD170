using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple bowling lane logic, is triggered externally by buttons that are routed
/// to the InitialiseRound, TalleyScore and ResetRack.
/// 
/// Future work;
///   Use the timer in update to limit how long a player has to bowl,
///   Detect that the player/ball is 'bowled' from behind the line
/// </summary>
public class BowlingLaneBehaviour : MonoBehaviour
{
    public GameObject pinPrefab;
    public GameObject bowlingBall;
    public Transform[] pinSpawnLocations;
    public Transform defaultBallLocation;
    //TODO; we need a way of tracking the pins that are used for scoring and so we can clean them up


    [ContextMenu("InitialiseRound")]
    public void InitialiseRound()
    {
        //TODO; need to move or init or create pins for a round of bowling, most likely to include some of the following;
        /*
        foreach (var pinLoc in pinSpawnLocations)
        {
            var newPin = Instantiate(pinPrefab, pinLoc.position, pinLoc.rotation);
        }
        */
    }

    public void BallReachedEnd()
    {
        //TODO; this needs to return the ball to the ball feed so the player could bowl again or at least clean ups
    }

    [ContextMenu("TalleyScore")]
    public void TalleyScore()
    {
      //TODO; determine score and get that information out to a checklist item, either via event or directly
    }

    [ContextMenu("ResetRack")]
    public void ResetRack()
    {
        //TODO; clean up all objects created by the bowling lane, preparing for a new round of bowling to occur
    }

    protected void Update()
    {
        
    }
}
