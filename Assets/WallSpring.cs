using UnityEngine;

public class WallSpring : MonoBehaviour
{
    private BoxCollider2D collider;
    public float springBounce = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement pm = collision.GetComponent<PlayerMovement>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (pm != null && rb != null)
            {
                pm.bounceOverride = true;

                float direction = pm.isFacingRight ? -1f : 1f;
                rb.AddForce(new Vector2(direction * springBounce, 0f), ForceMode2D.Impulse);
            }
        }
    }
}
