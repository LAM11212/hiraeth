using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Canvas cv;
    public Transform player;
    private bool isFollowing;

    private void Update()
    {
        if (isFollowing && player != null)
        {
            cv.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            isFollowing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isFollowing = false;
        }
    }
}
