using UnityEngine;

public class GameMenu : MonoBehaviour {

    public void Play()
    {
        // Find which map is selected!

        if (FindObjectOfType<MapSwitcher>() != null)
            FindObjectOfType<LevelLoader>().LoadLevel(FindObjectOfType<MapManager>().maps[FindObjectOfType<MapSwitcher>().SelectedMap].SceneIndex);
        else
            FindObjectOfType<LevelLoader>().LoadLevel(1);
    }

}
