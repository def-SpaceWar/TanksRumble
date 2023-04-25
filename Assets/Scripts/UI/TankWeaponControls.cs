using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TankWeaponControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public bool isTouched;

    public int tankNumber;
    public TankWeapon tankWeapon;

    private Text m_Text;

    public bool isInitialized;
    public bool isAI;

    private void Awake()
    {
        if (PlayerPrefs.GetInt($"player{tankNumber}", 1) == 0)
        {
            Destroy(gameObject);
            Destroy(this);
        }

        m_Text = GetComponentInChildren<Text>();
        m_Text.text = "Shoot!\nKills:  0";
    }

    private void Start()
    {
        Color anotherColor = GameSettings.Instance.PlayerColors[tankNumber - 1];
        anotherColor.a = 0.5f;

        GetComponent<Image>().color = anotherColor;
    }

    public void Initialize()
    {
        if (PlayerPrefs.GetInt($"player{tankNumber}AI", 0) == 1)
        {
            GetComponent<Button>().enabled = false;
            isAI = true;
        }

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized)
            return;

        switch (tankNumber) {
            case 1:
                if (Input.GetKeyDown("w")) {
                    isTouched = true;
                }

                if (Input.GetKeyUp("w")) {
                    isTouched = false;
                    tankWeapon.Shoot();
                }
                break;
            case 2:
                if (Input.GetKeyDown("o")) {
                    isTouched = true;
                }

                if (Input.GetKeyUp("o")) {
                    isTouched = false;
                    tankWeapon.Shoot();
                }
                break;
        }

        if (tankWeapon.jammed)
        {
            if (GetComponent<Button>() != null)
            {
                GetComponent<Button>().enabled = false;
            }
        }

        m_Text.text = $"Shoot!\nKills:  {tankWeapon.kills}";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isAI)
            return;

        isTouched = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isAI)
            return;

        if (isTouched)
        {
            isTouched = false;
            tankWeapon.Shoot();
        }
    }

}
