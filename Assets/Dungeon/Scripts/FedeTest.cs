using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
public class FedeTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ssa());
    }
    IEnumerator ssa()
    {
        yield return new WaitForSecondsRealtime(1f);
        FadeManager.Instance.FadeOut();
        yield return new WaitForSecondsRealtime(1.5f);
        FadeManager.Instance.FadeIn();
    }
}
