using UnityEngine;

public class LaserRoomController : MonoBehaviour
{
    [SerializeField] private Laser[] lasersInRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Player"))
        {
            foreach (Laser laser in lasersInRoom)
            {
                laser.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (Laser laser in lasersInRoom)
            {
                laser.gameObject.SetActive(false);
            }
        }
    }
}

