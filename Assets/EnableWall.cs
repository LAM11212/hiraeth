using UnityEngine;

public class EnableWall : MonoBehaviour
{
    public BoxCollider2D wallTrigger;
    public MovingWall wall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wall.StartWall();
            wallTrigger.enabled = false;
        }
    }
}
