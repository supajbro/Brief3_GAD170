using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility to allow for bulk OR comparison of tag checks on a given GO or collection of GOs
/// 
/// NOTE: Provided with framework, no modification required
/// </summary>
[System.Serializable]
public class TagCollection
{
    [SerializeField]
    protected string[] tagsToCheck = new string[0];

    public bool CheckAll(string toCompare)
    {
        for (int i = 0; i < tagsToCheck.Length; i++)
        {
            if (tagsToCheck[i] == toCompare)
                return true;
        }

        return tagsToCheck.Length != 0 ? false : true;
    }

    public bool CheckAll(string[] toCompare)
    {
        for (int i = 0; i < tagsToCheck.Length; i++)
        {
            for (int j = 0; j < toCompare.Length; j++)
            {
                if (tagsToCheck[i] == toCompare[j])
                    return true;
            }
        }

        return tagsToCheck.Length != 0 ? false : true;
    }

    public bool CheckAll(GameObject toCompare)
    {
        for (int i = 0; i < tagsToCheck.Length; i++)
        {
            if (toCompare.CompareTag(tagsToCheck[i]))
                return true;
        }

        return tagsToCheck.Length != 0 ? false : true;
    }

    public bool CheckAll(GameObject[] toCompare)
    {
        for (int i = 0; i < tagsToCheck.Length; i++)
        {
            for (int j = 0; j < toCompare.Length; j++)
            {
                if (toCompare[j].CompareTag(tagsToCheck[i]))
                    return true;
            }
        }

        return tagsToCheck.Length != 0 ? false : true;
    }
}
