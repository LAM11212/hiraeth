using UnityEngine;

public class TimelineSignalBridge : MonoBehaviour
{
    public void NextLevel()
    {
        if (GameController.instance != null)
            GameController.instance.NextLevel();
    }
}
