using UnityEngine;

public class HorizObstacleMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float moveDistance = 10f;
    public float startX;
    public bool right;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = rb.position.x;
    }

    private void FixedUpdate()
    {
        if(right)
        {
            float newX = startX + Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            rb.MovePosition(new Vector2(newX, rb.position.y));
        }
        else if(!right)
        {
            float newX = startX - Mathf.PingPong(Time.time * moveSpeed, moveDistance);
            rb.MovePosition(new Vector2(newX, rb.position.y));
        }
        
    }
}
