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
            player.SetActive(false);
            cutscenePlayer.SetActive(true);
            scaryMadeline.SetActive(true);
            cutsceneDirector.Play();
            Destroy(gameObject);
        }
    }
}
