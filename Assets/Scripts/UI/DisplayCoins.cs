using UnityEngine;
using UnityEngine.UI;

public class DisplayCoins : MonoBehaviour {

    private Text m_Text;

    private void Start()
    {
        m_Text = GetComponent<Text>();
    }

    private void Update()
    {
        m_Text.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }

}
