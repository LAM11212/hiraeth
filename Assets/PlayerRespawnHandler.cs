using UnityEngine;

public class PlayerRespawnHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelBound") || collision.CompareTag("Obstacle"))
        {
            GameController.instance.PlayerRespawn();
        }
        else if (collision.CompareTag("SpawnPoint"))
        {
            GameController.instance.SetNewSpawn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            GameController.instance.ClearSpawnFlag();
        }
    }
}
