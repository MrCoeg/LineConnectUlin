using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveUI : MonoBehaviour
{
    public CanvasSize canvasSize;
    public float m_width, m_height;
    public float m_widthScaler, m_heightScaler;
    public Vector2 m_position;
    public Vector2 m_rect;
    private float canvasWidth, canvasHeight;
    private void Start()
    {
        m_rect = GetComponent<RectTransform>().sizeDelta;
        m_width = m_rect.x;
        m_height = m_rect.y;
        canvasWidth = canvasSize.width;
        canvasHeight = canvasSize.height;
        m_widthScaler = m_width / canvasWidth;
        m_heightScaler = m_height / canvasHeight;

    }

    private void Update()
    {
        
        m_rect = new Vector2(m_widthScaler * canvasSize.width , m_heightScaler * canvasSize.height);
        GetComponent<RectTransform>().sizeDelta = m_rect;
    }

}
