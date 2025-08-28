using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderTransition : MonoBehaviour
{
    public BoxCollider2D transitionCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("ts is called");
        }
    }
}
