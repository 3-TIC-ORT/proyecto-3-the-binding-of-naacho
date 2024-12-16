using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemUnlocker : MonoBehaviour
{
    private string[] allNormalItemsNames;
    private string[] allSpecialItemsNames;
    private string[] unlockedNormalItemsNames;
    private string[] unlockedSpecialItemsNames;
    public TextMeshProUGUI itemText;
    [SerializeField] float timeBeforeChangingScene;
    void Start()
    {
        FadeManager.Instance.FadeOut();
        StartCoroutine(UpdateItemsUnlocked());
    }

    IEnumerator UpdateItemsUnlocked()
    {
        allNormalItemsNames = RoomTemplates.allNormalItemsNames;
        allSpecialItemsNames = RoomTemplates.allSpecialItemsNames;
        unlockedNormalItemsNames = SaveManager.LoadItemsUnlocked().normalItemsNames;
        unlockedSpecialItemsNames = SaveManager.LoadItemsUnlocked().specialItemsNames;

        List<string> newNomalItems = FilterLockedItems(allNormalItemsNames, unlockedNormalItemsNames);
        List<string> newSpecialItems = FilterLockedItems(allSpecialItemsNames, unlockedSpecialItemsNames);

        if (newNomalItems.Count > 0 && newSpecialItems.Count > 0)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                AddNewItem(newNomalItems.ToArray(), unlockedNormalItemsNames, true);
            }
            else
            {
                AddNewItem(newSpecialItems.ToArray(), unlockedSpecialItemsNames, false);
            }
        }
        else
        {
            if (newNomalItems.Count > 0) AddNewItem(newNomalItems.ToArray(), unlockedNormalItemsNames, true);
            else if (newSpecialItems.Count > 0) AddNewItem(newSpecialItems.ToArray(), unlockedSpecialItemsNames, false);
            else
            {
                itemText.text = "No hay más objetos por desbloquear";
            }
        }

        while (!FadeManager.Instance.fadeOutFinished)
        {
            yield return null;
        }
        StartCoroutine(ChangeScene(timeBeforeChangingScene));
    }

    private List<string> FilterLockedItems(string[] allItems, string[] unlockedItems)
    {
        return allItems.Where(item => !unlockedItems.Contains(item)).ToList();
    }
    
    private void AddNewItem(string[] candidatesItems, string[] unlockedItemsNames, bool isNormal)
    {
        int rand = Random.Range(0, candidatesItems.Length);
        string newItem = candidatesItems[rand];
        List<string> localList = new List<string>(unlockedItemsNames);
        localList.Add(newItem);
        if (isNormal)
        {
            SaveManager.SaveItemsUnlocked(localList.ToArray(), unlockedSpecialItemsNames);
        }
        else
        {
            SaveManager.SaveItemsUnlocked(unlockedNormalItemsNames, localList.ToArray());
        }
        itemText.text = $"La tortuga se comió un {newItem}";
    }

    IEnumerator ChangeScene(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        FadeManager.Instance.FadeIn();
        while (!FadeManager.Instance.fadeInFinished)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }
}
