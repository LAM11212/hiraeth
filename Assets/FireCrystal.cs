using UnityEngine;

public class FireCrystal : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Collider2D collider;
    public float respawnTime = 5f;
    private bool isCollected = false;
    private float explodeTimer = 5f;
    [SerializeField] PlayerMovement pm;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    public void Respawn()
    {
        explodeTimer = 5f;
        renderer.enabled = true;
        collider.enabled = true;
    }

    private void Collect(PlayerMovement pm)
    {
        if(pm.dashCount <= 0)
        {
            pm.dashCount++;
        }
        pm.moveSpeed += 2f;
        pm.jumpPower += 2f;
        pm.dashDistance += 2f;
        renderer.enabled = false;
        collider.enabled = false;
        pm.isMarkedForDeath = true;
        Invoke(nameof(Explode), explodeTimer);
        Invoke(nameof(Respawn), respawnTime);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
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

    private void Explode()
    {
        if(!pm.isMarkedForDeath) return;
        GameController gc = pm.GetComponent<GameController>();
        if(gc != null)
        {
            pm.moveSpeed = 5f;
            pm.jumpPower = 8.2f;
            pm.dashDistance = 2.57f;
            gc.Respawn();
        }
    }
}
