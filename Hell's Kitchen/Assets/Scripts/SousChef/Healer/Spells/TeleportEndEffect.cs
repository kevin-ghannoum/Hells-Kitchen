using System.Collections;
using UnityEngine;

public class TeleportEndEffect : MonoBehaviour
{
    [SerializeField] Transform sphere;
    [SerializeField] Light pointLight;

    void Start()
    {
        StartCoroutine(ScaleOverTime(0.35f, sphere, new Vector3(4, 4, 4)));
        Destroy(gameObject, 4);
    }

    bool finishedFirst = false;
    bool startedNext = false;
    private void Update()
    {
        if (finishedFirst && !startedNext)
        {
            StartCoroutine(ScaleOverTime(0.35f, sphere, new Vector3(0, 0, 0)));
            startedNext = true;
        }
        if (startedNext)
        {
            pointLight.intensity -= 2 * Time.deltaTime;
        }
    }
    IEnumerator ScaleOverTime(float time, Transform target, Vector3 desiredScale)
    {
        Vector3 originalScale = target.localScale;
        Vector3 destinationScale = desiredScale;

        float currentTime = 0.0f;

        do {
            target.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
        finishedFirst = true;
    }
}
