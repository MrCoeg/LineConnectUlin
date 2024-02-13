using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSize : MonoBehaviour
{
    public float width, height;
    public Vector2 pos;

    public Image image;
    private void Awake()
    {
        width = image.rectTransform.rect.size.x;
        height = image.rectTransform.rect.size.y;
    }
    private void Update()
    {
        width = image.rectTransform.rect.size.x;
        height = image.rectTransform.rect.size.y;

    }

}
