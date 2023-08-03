using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMask : MonoBehaviour
{
    [SerializeField]
    Transform circle;

    float fadeTime = 0.5f;
    float timer = 0f;

    Vector3 minScale = new Vector3(5f, 5f, 0f);
    Vector3 maxScale = new Vector3(25f, 25f, 0f);

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        circle.localScale = minScale;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            circle.localScale = Vector3.Lerp(minScale, maxScale, timer / fadeTime);
            yield return null;
        }
        timer = 0f;
        circle.localScale = maxScale;
        Debug.Log("FadeIn End");
    }

    public IEnumerator FadeOut()
    {
        circle.localScale = maxScale;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            circle.localScale = Vector3.Lerp(maxScale, minScale, timer / fadeTime);
            yield return null;
        }
        timer = 0f;
        circle.localScale = minScale;
        Debug.Log("FadeIn End");
    }

}
