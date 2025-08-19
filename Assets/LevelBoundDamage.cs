using UnityEngine;

public class LevelBoundDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.instance.PlayerRespawn();
        }
    }
}
