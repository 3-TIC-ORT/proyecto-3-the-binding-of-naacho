using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RayosExt : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float damage;
    public Enemy enemy;
    public Transform[] points;
    public Vector3[] lastPositions;
    private Vector3 currentPosition;
    private bool arrived = false;
    void Start()
    {
        transform.position = lastPositions[0];
        currentPosition = transform.position;
        lineRenderer = GetComponent<LineRenderer>();
        DOTween.To(() => currentPosition, x => currentPosition = x, points[1].position, 0.15f).onComplete+=()=> 
        {
            arrived = true;
            enemy.Damage(damage);
            Color startColor = lineRenderer.startColor;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
            Color2 startColor2 = new Color2(lineRenderer.startColor, lineRenderer.endColor);
            Color2 endColor2 = new Color2(endColor, endColor);
            lineRenderer.DOColor(startColor2, endColor2, 1f).onComplete+=()=> { Destroy(gameObject); };
        };
        SetUpLine(points);
    }
    
    void SetUpLine(Transform[] points)
    {
        lineRenderer.positionCount = points.Length;
        this.points = points;
    }

    private void Update()
    {
        Vector3 posA = lastPositions[0];
        Vector3 posB = lastPositions[1];
        if (points[0]!=null) lastPositions[0] = points[0].position;
        if (points[1]!=null) lastPositions[1] = points[1].position;
        lineRenderer.SetPosition(0, posA);
        if (!arrived) lineRenderer.SetPosition(1, currentPosition);
        else lineRenderer.SetPosition(1, posB);   
    }
}
