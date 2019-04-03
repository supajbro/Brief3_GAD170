using System;
using UnityEngine;

/// <summary>
/// Base class items within the checklist. Typically results in a count that gets updated when a
/// GameEvent is fired. Then is required to invoke the game event for a checklistitem changing
/// 
/// See: HoopsCheckListItem for example
/// 
/// NOTE: Provided with framework, no modification required
/// 
/// Future work:
///   It would make sense to have some scaffolding around the gameevent for checklist item changed
///     would complicate this script a little bit more.
/// </summary>
abstract public class CheckListItem : MonoBehaviour
{
    abstract public bool IsComplete { get;}

    abstract public float GetProgress();

    abstract public string GetTaskReadout();

    abstract public string GetStatusReadout();
}