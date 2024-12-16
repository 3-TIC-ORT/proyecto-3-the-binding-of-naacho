using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class StatsUpdater : MonoBehaviour
{
    private float cronometer;
    private string[] itemsTaken;
    public TextMeshProUGUI currentCronometerText;
    public TextMeshProUGUI currentItemsTakenText;
    public TextMeshProUGUI bestCronometerText;
    public TextMeshProUGUI bestItemsTakenText;



    // ESTE SCRIPT NO HACE FADE OUT NI CARGA OTRA ESCENA PORQUE YA LO HACE EL SCRIPT ItemUnlocker


    void Start()
    {
        cronometer = GameManager.cronometer;
        itemsTaken = RoomTemplates.itemsTakenNames.ToArray();
        UpdateStats();
    }

    void UpdateStats()
    {
        FinishStats pastData = SaveManager.LoadFinishStats();
        if (cronometer <= pastData.cronometer) SaveManager.SaveNewFinishStats(cronometer, itemsTaken);
        currentCronometerText.text = cronometer.ToString();
        string itemsTakenSingleString = "";
        if (itemsTaken.Length > 0) itemsTakenSingleString=GetItemsTakenString(itemsTaken);
        else itemsTakenSingleString = "Ningún item agarrado";
        currentItemsTakenText.text = itemsTakenSingleString;
        
        pastData = SaveManager.LoadFinishStats();
        bestCronometerText.text = pastData.cronometer.ToString();
        itemsTakenSingleString = "";
        if (pastData.itemsTaken.Length > 0) itemsTakenSingleString = GetItemsTakenString(pastData.itemsTaken);
        else itemsTakenSingleString = "Ningún item agarrado";
        bestItemsTakenText.text = itemsTakenSingleString;
        
    }
    private string GetItemsTakenString(string[] itemsTakenP)
    {
        string itemsTakenSingleString = "";
        List<ItemTaken> itemsTakenClassList = new List<ItemTaken>();
        foreach (string itemTaken in itemsTakenP)
        {
            ItemTaken itemTakenClass = new ItemTaken(1, itemTaken);
            if (itemsTakenClassList.Find(i => i.itemTakenName == itemTakenClass.itemTakenName) == null)
            {
                itemsTakenClassList.Add(itemTakenClass);
            }
            else
            {
                itemsTakenClassList.Find(i => i.itemTakenName == itemTakenClass.itemTakenName).timesTaken += 1;
            }
        }
        foreach (ItemTaken itemTaken in itemsTakenClassList)
        {
            if (itemTaken.timesTaken > 1)
            {
                itemsTakenSingleString += $"{itemTaken.itemTakenName} * {itemTaken.timesTaken.ToString()}, ";
            }
            else itemsTakenSingleString += $"{itemTaken.itemTakenName}, ";
        }
        return itemsTakenSingleString;
    }
    class ItemTaken
    {
        public int timesTaken;
        public string itemTakenName;

        public ItemTaken(int timesTakenP, string itemTakenNameP)
        {
            timesTaken = timesTakenP;
            itemTakenName = itemTakenNameP;
        }
    }
}
