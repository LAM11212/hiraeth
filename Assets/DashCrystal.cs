using UnityEngine;

public class DashCrystal : MonoBehaviour
{
    public float respawnTime = 5f;
    private bool isCollected = false;
    private SpriteRenderer renderer;
    private Collider2D collider;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    public void Collect(PlayerMovement pm)
    {
        if (pm.dashCount > 0) return;
        else if(pm.dashCount <= 0)
        {
            pm.dashCount++;
            pm.wallClimbTimer = 3f;
            renderer.enabled = false;
            collider.enabled = false;
            isCollected = true;
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    public void Respawn()
    {
        renderer.enabled = true;
        collider.enabled = true;
        isCollected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement pm = collision.GetComponent<PlayerMovement>();
            if(pm != null)
            {
                Collect(pm);
            }
            
        }
    }

    public void ForceRespawn()
    {
        CancelInvoke();
        Respawn();
    }
}
