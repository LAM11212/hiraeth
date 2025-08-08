using System.Collections;
using UnityEngine;

public class CrumblingBlock : MonoBehaviour
{
    public float DisappearTime = 1f;
    private float respawnTimer = 3f;

    private SpriteRenderer renderer;
    private Collider2D[] colliders;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0;
        Color originalColor = renderer.color;
        while ( t < DisappearTime)
        {
            renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t / DisappearTime));
            t += Time.deltaTime;
            yield return null;
        }

        renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        foreach(Collider2D col in colliders)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(respawnTimer);

        foreach(Collider2D col in colliders)
        {
            col.enabled = true;
        }
        renderer.color = originalColor;
    }
}
