using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthFlash : MonoBehaviour
{
    Image image;

    public float fadeTarget = 50f;
    public float fadeTime = 1.0f;

    private bool fading = false;

    float timer = 0;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void fadeIn()
    {
        fading = true;
    }


    private void Update()
    {
        if (!fading)
        {
            var color = image.color;
            var targetColor = color;
            targetColor.a = 0;

            image.color = Color.Lerp(color, targetColor, fadeTime * Time.deltaTime);
        }
        else
        {

            timer += Time.deltaTime;

            var color = image.color;
            var targetColor = color;
            targetColor.a = fadeTarget;

            image.color = Color.Lerp(color, targetColor, fadeTime * Time.deltaTime);

            if (timer <= 1.0f)
            {
                fading = false;
                timer = 0;
            }
        }
    }

}
