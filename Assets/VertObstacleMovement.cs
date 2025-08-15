using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float moveDistance = 10f;
    public float startY;
    public bool down;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startY = rb.position.y;
    }

    private void FixedUpdate()
    {
        if(down)
        {
            float newY = startY - Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            rb.MovePosition(new Vector2(rb.position.x, newY));
        }
        else if(!down)
        {
            float newY = startY + Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            rb.MovePosition(new Vector2(rb.position.x, newY));
        }
    }
}

