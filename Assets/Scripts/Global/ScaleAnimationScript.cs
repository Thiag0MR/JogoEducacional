using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimationScript : MonoBehaviour
{
    private Vector3 minScale;
    public Vector3 maxScale;
    public bool onStart;
    public bool repeatable;
    public float speed = 2f;
    public float duration = 5f;

    private void Start()
    {
        if (onStart)
        {
            StartCoroutine(ScaleAfterRandomTime());
        }
    }
    private IEnumerator ScaleAfterRandomTime()
    {
        yield return StartCoroutine(WaitRandomTime());

        minScale = transform.localScale;
        while (repeatable)
        {
            yield return RepeatLerp(minScale, maxScale, duration);
            yield return RepeatLerp(maxScale, minScale, duration);
        }
    }

    public IEnumerator Scale()
    {
        minScale = transform.localScale;
        yield return RepeatLerp(minScale, maxScale, duration);
        yield return RepeatLerp(maxScale, minScale, duration);
    }
    private IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    private IEnumerator WaitRandomTime()
    {
        float time = Random.Range(0, 1000) * gameObject.transform.GetSiblingIndex();
        yield return new WaitForSeconds(time/1000);
    }

}
