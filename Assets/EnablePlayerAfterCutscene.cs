using UnityEngine;

public class EnablePlayerAfterCutscene : MonoBehaviour
{
    public GameObject player;
    public bool wasEnabled = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasEnabled) return;
        if(collision.CompareTag("Player"))
        {
            EnablePlayer();
        }
    }
    public void EnablePlayer()
    {
        player.SetActive(true);
        wasEnabled = true;
        Debug.Log("this was hit");
    }
}
