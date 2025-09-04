using Unity.VisualScripting;
using UnityEngine;

public class EnableLasers : MonoBehaviour
{
    [SerializeField] private GameObject[] lasers;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach(GameObject ls in lasers)
            {
                ls.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject ls in lasers)
            {
                ls.SetActive(false);
            }
        }
    }
}
