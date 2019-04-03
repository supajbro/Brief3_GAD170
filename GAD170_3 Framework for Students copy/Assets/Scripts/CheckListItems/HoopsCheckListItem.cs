using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks number of shots up to a target amount. Is told directly by Hoop via Unity Event. 
/// </summary>
public class HoopsCheckListItem : CheckListItem
{
    public int numberOfRequiredHoops;
    public int numberOfHoopsScored;

    public override bool IsComplete { get { return numberOfHoopsScored == numberOfRequiredHoops; } }

    public override float GetProgress()
    {
        return (float)numberOfHoopsScored / (float)numberOfRequiredHoops;
    }

    public override string GetStatusReadout()
    {
        return numberOfHoopsScored.ToString() + " / " + numberOfRequiredHoops.ToString();
    }

    public override string GetTaskReadout()
    {
        return "Shoot some hoops";
    }

    public void OnBasketScored()
    {
        if (numberOfHoopsScored < numberOfRequiredHoops)
        {
            var ourData = new GameEvents.CheckListItemChangedData();
            ourData.item = this;
            ourData.previousItemProgress = GetProgress();

            numberOfHoopsScored++;

            GameEvents.InvokeCheckListItemChanged(ourData);
        }
    }
}
