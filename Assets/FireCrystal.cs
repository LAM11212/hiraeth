using UnityEngine;

public class FireCrystal : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Collider2D collider;
    public float respawnTime = 5f;
    private bool isCollected = false;
    private float explodeTimer = 5f;
    [SerializeField] private GameController gc;
    private PlayerMovement player;

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
        isCollected = false;
    }

    private void Collect(PlayerMovement pm)
    {
        if(isCollected) return;
        isCollected = true;
        player = pm;
        if (pm.dashCount <= 0)
        {
            pm.dashCount++;
        }
        pm.moveSpeed += 2f;
        pm.jumpPower += 2f;
        pm.dashDistance += 2f;
        pm.wallClimbTimer = 3f;
        pm.wallClimbJumpsRemaining = 3;
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

    private void Explode()
    {
        Debug.Log("Method called");
        if(player != null && player.isMarkedForDeath)
        {
            Debug.Log("First if crossed");
            player.isMarkedForDeath = false;
            if(gc != null)
            {
                Debug.Log("Second if crossed, check game controller script");
                gc.PlayerRespawn();
            }
        }
    }

    public void CancelExplode()
    {
        CancelInvoke(nameof(Explode));
    }
}
