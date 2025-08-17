using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    [Header("Player Settings")]
    [SerializeField] private PlayerMovement pm;

    Vector2 startPos;
    private bool justSetNewSpawn;

    private DashCrystal[] crystals;
    private FireCrystal[] fireCrystals;
    private WaterCrystal[] waterCrystals;

    private float defaultMoveSpeed;
    private float defaultJumpPower;
    private float defaultDashDistance;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if (pm == null) 
            pm = Object.FindFirstObjectByType<PlayerMovement>();

        startPos = pm.transform.position;
        defaultMoveSpeed = pm.moveSpeed;
        defaultJumpPower = pm.jumpPower;
        defaultDashDistance = pm.dashDistance;

        crystals = Object.FindObjectsByType<DashCrystal>(FindObjectsSortMode.None);
        fireCrystals = Object.FindObjectsByType<FireCrystal>(FindObjectsSortMode.None);
        waterCrystals = Object.FindObjectsByType<WaterCrystal>(FindObjectsSortMode.None);
    }


    public void PlayerRespawn()
    {
        Respawn();
        foreach (DashCrystal crystal in crystals) crystal.ForceRespawn();
        foreach (FireCrystal fCrystal in fireCrystals) fCrystal.ForceRespawn();
        foreach (WaterCrystal wCrystal in waterCrystals) wCrystal.ForceRespawn();
    }

    public void Respawn()
    {
        pm.transform.position = startPos;
        ResetPlayerStats();
        pm.isMarkedForDeath = false;
        
    }

    public void SetNewSpawn()
    {
        if (justSetNewSpawn) return;
        startPos = pm.transform.position;
    }

    private void ResetPlayerStats()
    {
        pm.moveSpeed = defaultMoveSpeed;
        pm.jumpPower = defaultJumpPower;
        pm.dashDistance = defaultDashDistance;
    }

    public void ClearSpawnFlag()
    {
        justSetNewSpawn = false;
    }
}
