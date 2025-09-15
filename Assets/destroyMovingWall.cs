using UnityEngine;

public class destroyMovingWall : MonoBehaviour
{
    public MovingWall wall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Object.Destroy(wall.gameObject);
        }
    }
}
