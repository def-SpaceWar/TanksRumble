using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerEnterAndExitControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public int tankNumber;
    private string m_PlayerKey;

    private bool m_Pressed;
    private bool m_IsPlaying;
    private Text m_Text;

    private TankSwitcher tankSwitcher;

    private void Awake()
    {
        m_PlayerKey = $"player{tankNumber}";
        m_Text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(m_PlayerKey, 1) == 1)
            m_IsPlaying = true;

        if (transform.parent.GetComponentInChildren<TankSwitcher>() != null)
        {
            tankSwitcher = transform.parent.GetComponentInChildren<TankSwitcher>();
        }
        else
        {
            tankSwitcher = null;
        }
    }

    private void Update()
    {
        if (m_IsPlaying)
        {
            PlayerPrefs.SetInt(m_PlayerKey, 1);
            m_Text.text = "Playing!";

            if (tankSwitcher != null)
            {
                tankSwitcher.gameObject.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt(m_PlayerKey, 0);
            m_Text.text = "Not  Playing!";

            if (tankSwitcher != null)
            {
                tankSwitcher.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_Pressed)
        {
            m_Pressed = false;
            m_IsPlaying = !m_IsPlaying;
        }
    }

}
