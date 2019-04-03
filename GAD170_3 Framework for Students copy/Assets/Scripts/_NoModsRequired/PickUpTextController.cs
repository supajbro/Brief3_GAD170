using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Routes changes from item pickup events into an on screen text field
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
public class PickUpTextController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    private void OnEnable()
    {
        GameEvents.OnIteractiveItemChange += GameEvents_OnIteractiveItemChange;
        text.text = string.Empty;
    }

    private void GameEvents_OnIteractiveItemChange(GameEvents.InteractiveItemData data)
    {
        if(data.type == GameEvents.InteractiveItemData.InteractionType.Presented)
        {
            if(data.item != null)
            {
                text.text = data.item.name;
            }
            else
            {
                text.text = string.Empty;
            }
        }
    }

    private void OnDisable()
    {
        GameEvents.OnIteractiveItemChange -= GameEvents_OnIteractiveItemChange;
    }
}
