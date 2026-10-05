using System;
using UnityEngine;
using UnityEngine.UI;

public class AlphaLerp : MonoBehaviour
{
    public RawImage img;
    public Transform endLerpPos;
    private bool isLerping = false;
    public Transform player;

    private void Start()
    {
        Color color = img.color;
        color.a = 0f;
        img.color = color;
    }

    public void Update()
    {
        if (!isLerping) return;

        float distance = Vector2.Distance(player.position, endLerpPos.position);
        float alpha = Mathf.Clamp01(1f - distance / 10f);
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Console.WriteLine("lerp activated");
            isLerping = true;
        }
    }
}
