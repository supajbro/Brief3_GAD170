using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Store of checklist items, added via inspector. It also deals with the display of the task info
/// via a textmeshpro element.
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
public class CheckList : MonoBehaviour
{
    public bool initOnStart = true;
    public List<CheckListItem> checkListItems = new List<CheckListItem>();
    public RectTransform rootItemPanel;
    public GameObject itemTextPrefab;
    public List<TMPro.TextMeshProUGUI> checkListTexts = new List<TMPro.TextMeshProUGUI>();
    public Gradient completionGradient;
    public AudioEffectSO progressSFX, lostProgressSFX, completeSFX;

    private void Start()
    {
        if (initOnStart)
            Init();
    }

    public void Init()
    {
        foreach (var item in checkListTexts)
        {
            Destroy(item.gameObject);
        }
        checkListTexts.Clear();

        foreach (var item in checkListItems)
        {
            var newItemGO = Instantiate(itemTextPrefab, rootItemPanel);
            var newItemText = newItemGO.GetComponent<TMPro.TextMeshProUGUI>();
            if(newItemText != null)
            {
                checkListTexts.Add(newItemText);
            }
            else
            {
                Debug.LogError("Supplied itemTextPrefab does not have a TMPro.TextMeshProUGUI on it");
            }
        }

        RefreshAllTexts();
    }

    protected void RefreshAllTexts()
    {
        for (int i = 0; i < checkListItems.Count; i++)
        {
            RefreshItemText(i);
        }
    }

    protected void RefreshItemText(int index)
    {
        //the lists should match by index if they don't something has already gone very wrong
        var item = checkListItems[index];
        var txt = checkListTexts[index];

        txt.color = completionGradient.Evaluate(item.GetProgress());
        string line = string.Empty;
        if (item.IsComplete)
        {
            line += "<s>";
        }
        line += "<b>" + item.GetTaskReadout() + "</b> : " + item.GetStatusReadout();
        if (item.IsComplete)
        {
            line += "</s>";
        }

        txt.text = line;
    }

    private void OnEnable()
    {
        GameEvents.OnCheckListItemChanged += GameEvents_OnCheckListItemChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnCheckListItemChanged -= GameEvents_OnCheckListItemChanged;
    }

    private void GameEvents_OnCheckListItemChanged(GameEvents.CheckListItemChangedData data)
    {
        var locatedIndex = checkListItems.IndexOf(data.item);

        if(locatedIndex != -1)
        {
            RefreshItemText(locatedIndex);
        }

        if(data.item.IsComplete)
        {
            completeSFX.Play2D();
        }
        else if (data.previousItemProgress > data.item.GetProgress())
        {
            lostProgressSFX.Play2D();
        }
        else
        {
            progressSFX.Play2D();
        }
    }
}
