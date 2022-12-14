using UnityEngine;
using UnityEngine.UI;

public class SupportMeToggle : MonoBehaviour {

    public Toggle toggle;
    public Text text;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("IsSupporting", 0) == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    private void Update()
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetFloat("IsSupporting", 1);
            text.color = Color.green;
        }
        else
        {
            PlayerPrefs.SetFloat("IsSupporting", 0);
            text.color = Color.red;
        }
    }

}
