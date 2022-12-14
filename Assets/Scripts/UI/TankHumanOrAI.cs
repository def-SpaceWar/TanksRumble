using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TankHumanOrAI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public int tankNumber;

    private bool m_Pressed;
    private bool m_IsHuman;
    private string m_PlayerKey;
    private Text m_Text;

    private void Awake()
    {
        m_PlayerKey = $"player{tankNumber}AI";
        m_Text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(m_PlayerKey, 0) == 0)
            m_IsHuman = true;
    }

    private void Update()
    {
        if (m_IsHuman)
        {
            PlayerPrefs.SetInt(m_PlayerKey, 0);
            m_Text.text = "Human!";
        }
        else
        {
            PlayerPrefs.SetInt(m_PlayerKey, 1);
            m_Text.text = "AI!";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!m_Pressed) return;
        m_Pressed = false;
        m_IsHuman = !m_IsHuman;
    }

}
