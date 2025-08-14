using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float moveDistaqnce = 10f;
    public float startY;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startY = rb.position.y;
    }

    private void FixedUpdate()
    {
        float newY = startY + Mathf.PingPong(Time.time * moveSpeed, moveDistaqnce);
        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}

