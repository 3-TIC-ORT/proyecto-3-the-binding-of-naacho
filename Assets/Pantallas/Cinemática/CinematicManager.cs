using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class CinematicManager : MonoBehaviour
{
    [SerializeField] string videoFileName;
    [SerializeField] string sceneName;
    public VideoPlayer myVideoPlayer;
    public bool changingScene;
    void Start()
    {
        FadeManager.Instance.FadeOut();
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        myVideoPlayer.url = videoPath;
        myVideoPlayer.Play();
        myVideoPlayer.loopPointReached += StartChangingScene;
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
        SceneManager.LoadScene(sceneName);
    }
}
