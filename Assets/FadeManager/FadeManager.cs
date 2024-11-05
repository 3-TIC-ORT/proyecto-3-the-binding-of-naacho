using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }
    public float fadeSpeed;
    public bool fadeInFinished;
    public bool fadeOutFinished;   
    private Image screen;
    private void Awake()
    {
        Application.targetFrameRate = 55;
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        screen = GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<Image>();
    }

    public void FadeIn()
    {
        fadeOutFinished = false;
        fadeInFinished = false;
        screen.enabled=true;
        screen.DOFade(1, fadeSpeed).SetEase(Ease.Linear).onComplete = () => 
        { 
            fadeInFinished = true; 
        };
    }
    public void FadeOut() 
    {
        fadeOutFinished = false;
        fadeInFinished = false;
        screen.DOFade(0, fadeSpeed).SetEase(Ease.Linear).onComplete = () => 
        {
            screen.enabled=false;
            fadeOutFinished = true; 
        };
    }
}
