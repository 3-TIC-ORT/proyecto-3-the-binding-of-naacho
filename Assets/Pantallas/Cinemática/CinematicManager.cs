using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class CinematicManager : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    void Start()
    {
        FadeManager.Instance.FadeOut();
        myVideoPlayer.loopPointReached += StartChangingScene;
    }
    private void StartChangingScene(VideoPlayer vp)
    {
        StartCoroutine(ChangeScene());
    }
    IEnumerator ChangeScene()
    {
        FadeManager.Instance.FadeIn();
        while (!FadeManager.Instance.fadeInFinished)
        {
            yield return null;
        }
        SceneManager.LoadScene("Mazmorras testing");
    }
}
