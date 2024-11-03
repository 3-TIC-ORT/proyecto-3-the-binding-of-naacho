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
        myVideoPlayer.loopPointReached += ChangeScene;
    }
    private void ChangeScene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Mazmorras testing");
    }
}
