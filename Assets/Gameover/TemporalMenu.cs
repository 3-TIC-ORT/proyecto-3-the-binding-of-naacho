using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TemporalMenu : MonoBehaviour
{
    public float timeBeforeChangingScene;
    public string scene;
    private void Start()
    {
        StartCoroutine(StartAndFinish());
    }
    IEnumerator StartAndFinish()
    {
        FadeManager.Instance.FadeOut();
        while (!FadeManager.Instance.fadeOutFinished)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(timeBeforeChangingScene);
        FadeManager.Instance.FadeIn();
        while (!FadeManager.Instance.fadeInFinished)
        {
            yield return null;
        }
        SceneManager.LoadScene(scene);
    }

    
        
    
}
