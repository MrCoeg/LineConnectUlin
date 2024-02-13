using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeAway : MonoBehaviour
{
    public float fadeTime;
    public float fadeSpeed;
    public Image image;

    private void Start()
    {
        StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        var time = fadeTime;
        var color = image.color;
        while (time >=0) {
            time -= Time.deltaTime * fadeSpeed;
            image.color = new Color(color.r, color.g, color.b, time);

        }
        yield return null;
    }
}
