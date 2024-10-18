using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraAim : MonoBehaviour
{
    private Transform playerPos;
    private CameraFollowingActivation cfa;
    private CinemachineVirtualCamera _camera;
    public float followSpeed;
    public float nearCamera;
    public float farCamera;
    [Tooltip("Es el tiempo de transición para expandir la cámara")]
    public float transitionSpeed;
    public float cameraShakeSpeed;
    [Tooltip("El tiempo en el cual todo está quieto sin moverse ni actuar después de haber transicioando de una room a otra")]
    public float pauseTime;
    private void Start()
    {
        cfa = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraFollowingActivation>();
        _camera = GameObject.Find("Camera").GetComponent<CinemachineVirtualCamera>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        if (CheckIfBigRoom() && _camera.m_Lens.OrthographicSize == nearCamera)
        {
            DOTween.To(() => _camera.m_Lens.OrthographicSize, x => _camera.m_Lens.OrthographicSize = x, farCamera, transitionSpeed);
        }
        else if (!CheckIfBigRoom() && _camera.m_Lens.OrthographicSize == farCamera)
        {
            DOTween.To(() => _camera.m_Lens.OrthographicSize, x => _camera.m_Lens.OrthographicSize = x, nearCamera, transitionSpeed);
        }
        if (GameManager.Instance.stop) return;
        if (cfa.followVertically)
        {
            transform.DOMoveY(playerPos.position.y, followSpeed);
        }
        if (cfa.followHorizontally)
        {
            transform.DOMoveX(playerPos.position.x, followSpeed);
        }
    }
    bool CheckIfBigRoom()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("BigRoomTrigger")) return true;
        }
        return false;
    }
}
