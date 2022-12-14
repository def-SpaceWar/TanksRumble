using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

    private Button m_Button;
    private Text m_Text;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        var players = 0;

        if (PlayerPrefs.GetInt("player1", 1) != 0) players++;
        if (PlayerPrefs.GetInt("player2", 1) != 0) players++;
        if (PlayerPrefs.GetInt("player3", 1) != 0) players++;
        if (PlayerPrefs.GetInt("player4", 1) != 0) players++;

        try
        {
            if (players < GameModeManager.GameModes[(GameModeType)PlayerPrefs.GetInt("GameMode")].MinPlayers)
            {
                m_Button.enabled = false;
                m_Text.text = $"{GameModeManager.GameModes[(GameModeType)PlayerPrefs.GetInt("GameMode")].MinPlayers} players min";
            }
            else
            {
                m_Button.enabled = true;
                m_Text.text = "Play";
            }
        }
        catch
        {
            return;
        }
    }

}
