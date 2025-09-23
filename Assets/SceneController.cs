using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public string sceneToLoad;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Object.Destroy(gameObject);
        }
    }
    public void NextLevel()
    {
        Debug.Log("this does get called");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        if(!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
        else
        {
            Debug.LogError("SceneController: sceneToLoad is emtpy or null");
        }
    }
}
