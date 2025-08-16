using UnityEngine;

public class WaterCrystal : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Collider2D collider;
    public float respawnTime = 5f;
    [SerializeField] PlayerMovement pm;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void Collect(PlayerMovement pm)
    {
        renderer.enabled = false;
        collider.enabled = false;
        pm.isMarkedForDeath = false;
        pm.moveSpeed = 5f;
        pm.jumpPower = 8.2f;
        pm.dashDistance = 2.57f;
        Invoke(nameof(Respawn), respawnTime);
    }

    private void Respawn()
    {
        renderer.enabled = true;
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (pm != null)
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
