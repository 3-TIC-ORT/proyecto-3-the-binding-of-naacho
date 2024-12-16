using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemsUnlocked
{
    public string[] normalItemsNames;
    public string[] specialItemsNames;

    public ItemsUnlocked(string[] normalItemsNamesP, string[] specialItemsNamesP)
    {
        normalItemsNames = normalItemsNamesP;
        specialItemsNames = specialItemsNamesP; 
    }
}
