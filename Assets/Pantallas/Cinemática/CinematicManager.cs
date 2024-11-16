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
#if !UNITY_STANDALONE_LINUX || !UNITY_EDITOR_LINUX
        myVideoPlayer.loopPointReached += StartChangingScene;
        print("HOLA");
#elif UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
        StartChangingScene();
#endif
    }
    private void StartChangingScene(VideoPlayer vp = null)
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
