using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TemporalMenu : MonoBehaviour
{
    public float transitionSpeed;
    private Image transitionScreen;
    public float timeBeforeChangingScene;
    public string scene;
    public float timeBeforeFadingOut;
    private void Start()
    {
        Invoke("FadeOut", timeBeforeFadingOut);
    }
    private void FadeOut()
    {
        transitionScreen = GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<Image>();
        if (transitionScreen != null ) 
        {
            transitionScreen.DOFade(0, transitionSpeed / 2).onComplete = () =>
            {
                transitionScreen.gameObject.SetActive(false);
                StartCoroutine(ChangeScene(scene));
            };
        }
    }
    IEnumerator ChangeScene(string scene)
    {
        yield return new WaitForSeconds(timeBeforeChangingScene);
        transitionScreen.gameObject.SetActive(true);
        transitionScreen.DOFade(1, transitionSpeed / 2).onComplete = () =>
        {
            SceneManager.LoadScene(scene);
        };
    }
}
