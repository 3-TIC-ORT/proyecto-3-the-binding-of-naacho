using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class MenuManager : MonoBehaviour
{
    public float transitionSpeed;
    private Image transitionScreen;

    private void Start()
    {
        transitionScreen = GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<Image>();
        transitionScreen.DOFade(0, transitionSpeed / 2).onComplete=()=> { transitionScreen.gameObject.SetActive(false);};
    }
    public void ChangeScene(string sceneName)
    {
        transitionScreen.gameObject.SetActive(true);
        transitionScreen.DOFade(1, transitionSpeed / 2).onComplete = () =>
        {
            SceneManager.LoadScene(sceneName);
        };
    }
    public void ExitGame() { Application.Quit(); }
}
