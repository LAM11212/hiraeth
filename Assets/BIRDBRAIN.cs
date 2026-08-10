using UnityEngine;
using UnityEngine.Video;

public class BIRDBRAIN : MonoBehaviour
{
    [SerializeField] private BoxCollider2D roomCollider;
    [SerializeField] private Canvas videoCanvas;
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.loopPointReached += VideoFinished;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            videoCanvas.gameObject.SetActive(true);
            videoPlayer.Play();
        }
    }

    private void VideoFinished(VideoPlayer player)
    {
        videoCanvas.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= VideoFinished;
    }
}
