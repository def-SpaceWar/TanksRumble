using UnityEngine;

public class PauseGroup : MonoBehaviour {

    private GameSettings m_GameSettings;

    private void Start()
    {
        m_GameSettings = FindObjectOfType<GameSettings>();
    }

    private void Update()
    {
        if (m_GameSettings.isWon)
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }

}
