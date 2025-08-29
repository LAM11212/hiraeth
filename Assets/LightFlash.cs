using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlash : MonoBehaviour
{
    public float flashDuration = 5f;
    public float flashInterval = 3f;
    private float flashTimer;
    public Light2D light;

    private void Awake()
    {
        if (light != null)
            light.intensity = 0f;
        flashTimer = flashInterval;
        
    }

    private void Update()
    {
        flashTimer -= Time.deltaTime;
        if (flashTimer <= 0f)
        {
            StartCoroutine(FlashLight());
            flashTimer = 3f;
        }
    }

    private IEnumerator FlashLight()
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            light.intensity = Mathf.Lerp(0f, 1f, t / flashDuration);
            yield return null;
        }

        t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            light.intensity = Mathf.Lerp(1f, 0f, t / flashDuration);
            yield return null;
        }

        light.intensity = 0f;
    }
}
