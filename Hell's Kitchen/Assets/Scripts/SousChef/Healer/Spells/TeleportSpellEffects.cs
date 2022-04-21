using System.Collections;
using UnityEngine;

public class TeleportSpellEffects : MonoBehaviour
{
    [SerializeField] LensFlare flare1;
    [SerializeField] LensFlare flare2;
    [SerializeField] Transform sphere;
    [SerializeField] GameObject endOfTeleport;
    [SerializeField] Transform sphere2;
    float startLightSize = 4f;
    float midLifeSize = 6f;
    float endLightSize = 0f;
    float lifetime = 0.35f;
    float _lifetime = 0f;
    float rateOfChange;

    float timePerLifeCycle;
    float sphereRateOfChange;
    private void Start()
    {
        timePerLifeCycle = lifetime / 4;
        rateOfChange = 2 / timePerLifeCycle;

        flare1.brightness = startLightSize;
        flare2.brightness = startLightSize;

        sphereRateOfChange = 4 / timePerLifeCycle;
        StartCoroutine(ScaleOverTime(timePerLifeCycle*3.5f, sphere));
    }
    
    IEnumerator ScaleOverTime(float time, Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 destinationScale = new Vector3(0f, 0f, 0f);

        float currentTime = 0.0f;

        do {
            target.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }
    void Update()
    {
        _lifetime += Time.deltaTime;
        if (_lifetime <= timePerLifeCycle * 1)
        { // beginning
            rateOfChange = 5f / timePerLifeCycle;
            flare1.brightness += rateOfChange * Time.deltaTime;
            flare2.brightness += rateOfChange * Time.deltaTime;
        }
        else if (_lifetime >= timePerLifeCycle * 1 && _lifetime <= timePerLifeCycle * 2)
        { // midlife
            rateOfChange = 7 / timePerLifeCycle;
            flare1.brightness -= rateOfChange * Time.deltaTime;
            flare2.brightness -= rateOfChange * Time.deltaTime;
        }
        else if (_lifetime >= timePerLifeCycle * 2 && _lifetime < timePerLifeCycle * 3)
        {// end of life
            rateOfChange = 7 / timePerLifeCycle;
            flare1.brightness -= rateOfChange * Time.deltaTime;
            flare2.brightness -= rateOfChange * Time.deltaTime;
            endOfTeleport.SetActive(true);
            StartCoroutine(ScaleOverTime(timePerLifeCycle * 3f, sphere2));
        }
        else 
        {// death
            Destroy(gameObject,3);
        }
    }
}
