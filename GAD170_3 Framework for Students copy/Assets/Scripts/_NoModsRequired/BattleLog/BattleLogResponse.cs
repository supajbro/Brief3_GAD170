using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple holder for a default log message, allows for configuring on a game object and triggering it via unity event or other means 
/// </summary>
public class BattleLogResponse : MonoBehaviour
{
    public DefaultLogMessage message; 

    public void LogMessageNow()
    {
        BattleLog.Log(message);
    }
}
