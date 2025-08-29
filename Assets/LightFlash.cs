using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlash : MonoBehaviour
{
    public float fadeDuration = 2f;
    public float lightDuration = 1f;
    private bool isFlashing = false;
    private float darkDuration = 2f;
    public Light2D light;

    private void Awake()
    {
        if (light != null)
            light.intensity = 0f;
        
    }

    private void Update()
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashLight());
        }
    }

    private IEnumerator FlashLight()
    {
        isFlashing = true;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            light.intensity = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        light.intensity = 1f;

        yield return new WaitForSeconds(lightDuration);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            light.intensity = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        light.intensity = 0f;

        yield return new WaitForSeconds(darkDuration);

        isFlashing = false;
    }
}
