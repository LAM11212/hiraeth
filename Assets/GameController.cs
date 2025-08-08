using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 startPos;
    private bool justSetNewSpawn;
    private DashCrystal[] crystals;
    void Start()
    {
        startPos = transform.position;
        crystals = Object.FindObjectsByType<DashCrystal>(FindObjectsSortMode.None);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("LevelBound") || collision.CompareTag("Obstacle"))
        {
            Respawn();
        }
        else if(collision.CompareTag("SpawnPoint"))
        {
            SetNewSpawn();
            justSetNewSpawn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            justSetNewSpawn = false;
        }
    }

        private void Respawn()
    {
        transform.position = startPos;
        foreach(DashCrystal crystal in crystals)
        {
            crystal.ForceRespawn();
        }
    }

    private void SetNewSpawn()
    {
        if (justSetNewSpawn) return;
        startPos = transform.position;
    }
}
