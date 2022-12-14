using UnityEngine;
using UnityEngine.UI;

public class TankSwitcher : MonoBehaviour {

    private int tankNumber;

    private string key;
    private int tankSelected;

    [SerializeField] private Image thumbnail;

    private void Start()
    {
        tankNumber = transform.parent.GetComponentInChildren<TankHumanOrAI>().tankNumber;

        foreach (Image image in GetComponentsInChildren<Image>())
        {
            Color color = transform.parent.GetComponentInChildren<TankHumanOrAI>().GetComponent<Image>().color;
            color.a = 1;

            image.color = color;
        }

        key = $"player{tankNumber}tank";

        foreach (TankType tankType in TankManager.TankTypes)
        {
            if (tankType.Name == PlayerPrefs.GetString(key, "Minitank"))
            {
                bool isRight = false;

                for (int i = 0; i < TankManager.TankTypes.Length; i++)
                {
                    if (TankManager.TankTypes[i].Name == tankType.Name)
                    {
                        tankSelected = i;
                        isRight = true;
                        break;
                    }
                }

                if (isRight) return;
            }
        }

        tankSelected = 0;
    }

    private void Update()
    {
        PlayerPrefs.SetString(key, TankManager.TankTypes[tankSelected].Name);
        thumbnail.sprite = TankManager.TankTypes[tankSelected].Thumbnail;
    }

    public void NextTank()
    {
        int idx = 0;

        for (int i = 0; i < TankManager.TankTypes.Length; i++)
        {
            if (i == tankSelected)
            {
                idx = i + 1;
                break;
            }
        }

        if (idx == TankManager.TankTypes.Length)
        {
            tankSelected = 0;
            return;
        }

        tankSelected = idx;
    }

    public void PreviousTank()
    {
        int idx = 0;

        for (int i = 0; i < TankManager.TankTypes.Length; i++)
        {
            if (i == tankSelected)
            {
                idx = i - 1;
                break;
            }
        }

        if (idx < 0)
        {
            tankSelected = TankManager.TankTypes.Length - 1;
            return;
        }

        tankSelected = idx;
    }

}
