using UnityEngine;
using UnityEngine.Playables;
public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;
    public GameObject player;
    public GameObject cutscenePlayer;
    public GameObject scaryMadeline;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(player != null)
            {
                player.SetActive(false);
            }

            if(cutscenePlayer != null)
            {
                cutscenePlayer.SetActive(true);
            }

            if (scaryMadeline != null)
            {
                scaryMadeline.SetActive(true);
            }
            cutsceneDirector.Play();
            Destroy(gameObject);
        }
    }
}
