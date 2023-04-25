using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TankMovementControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public bool isTouched;
    public bool isLeft;
    public int tankNumber;
    public TankMovement tankMovement;

    private Text m_Text;

    public bool isInitialized;
    public bool isAI;

    private void Awake()
    {
        if (PlayerPrefs.GetInt($"player{tankNumber}", 1) == 0)
        {
            Destroy(gameObject);
            Destroy(this);

            return;
        }

        m_Text = GetComponentInChildren<Text>();
        isLeft = true;

        m_Text.text = "Initializing!";
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
                if (Input.GetKeyDown("m")) {
                    isTouched = true;
                }
                if (Input.GetKeyUp("m")) {
                    isTouched = false;
                    isLeft = !isLeft;
                }
                break;
            case 2:
                if (Input.GetKeyDown("a")) {
                    isTouched = true;
                }
                if (Input.GetKeyUp("a")) {
                    isTouched = false;
                    isLeft = !isLeft;
                }
                break;
        }

        if (isTouched)
        {
            if (isLeft)
            {
                tankMovement.TurnLeft();
            }
            else
            {
                tankMovement.TurnRight();
            }
        }

        if (tankMovement.jammed)
        {
            m_Text.text = "Dead!";

            if (GetComponent<Button>() != null)
            {
                GetComponent<Button>().enabled = false;
            }
        }
        else
        {
            if (isLeft)
            {
                m_Text.text = "Turn  Left!";
            }
            else
            {
                m_Text.text = "Turn  Right!";
            }
        }
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
            isLeft = !isLeft;
        }
    }

}
