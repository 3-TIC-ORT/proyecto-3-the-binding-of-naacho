using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHitboxModifier : MonoBehaviour, ICanvasRaycastFilter
{
    [Tooltip("Textura donde los píxeles transparentes son una zona NO CLICKABLE, y donde los opacos sí lo son")]
    public Texture2D hitArea;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (hitArea == null)
            return true;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);

        Rect rect = rectTransform.rect;
        float x = (localPoint.x - rect.x) / rect.width * hitArea.width;
        float y = (localPoint.y - rect.y) / rect.height * hitArea.height;

        if (x >= 0 && x < hitArea.width && y >= 0 && y < hitArea.height)
        {
            Color pixel = hitArea.GetPixel((int)x, (int)y);
            return pixel.a > 0;
        }
        return false;
    }
}
