using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool IsPaused;

    public void Pause()
    {
        Time.timeScale = 0;
        IsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        IsPaused = false;
    }

    public void MenuScene()
    {
        Resume();
        var levelLoader = FindObjectOfType<LevelLoader>();
        levelLoader.LoadLevel(0); // Go to main menu
    }

}
