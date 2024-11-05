using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class MenuManager : MonoBehaviour
{ 
    private void Start() {
        StartCoroutine(startFadeOut());
    }
    IEnumerator startFadeOut()
    {
        yield return null;
        FadeManager.Instance.FadeOut();
    }
    public void StartChangeScene(string coroutineAndScene) 
    {
        string[] separados = coroutineAndScene.Split("-");
        string coroutineName = separados[0];
        string sceneName = separados[1];
        StartCoroutine(coroutineName,sceneName);
    }
    IEnumerator ChangeScene(string sceneName)
    {
        FadeManager.Instance.FadeIn();
        yield return null;
        while (!FadeManager.Instance.fadeInFinished)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame() { Application.Quit(); }
}
