using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class CinematicManager : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    public bool changingScene;
    void Start()
    {
        FadeManager.Instance.FadeOut();
#if !UNITY_STANDALONE_LINUX || !UNITY_EDITOR_LINUX
        myVideoPlayer.loopPointReached += StartChangingScene;
#elif UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
        StartChangingScene();
#endif
    }
    private void Update()
    {
        if (Input.anyKey && !changingScene && FadeManager.Instance.fadeOutFinished)
        {
            changingScene = true;
            StartCoroutine(ChangeScene());
        }
    }
    private void StartChangingScene(VideoPlayer vp = null)
    {
        if (!changingScene) StartCoroutine(ChangeScene());
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
