using UnityEngine;

public class SpringObject : MonoBehaviour
{
    private BoxCollider2D collider;
    public float springBounce = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, springBounce);
            }
        }
    }
}
