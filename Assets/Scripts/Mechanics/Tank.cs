using TMPro;
using UnityEngine;

public class Tank : MonoBehaviour {
    public int tankNumber; // Defined in the inspector so player 1, 2, 3, or 4!

    public float movePower;
    public float turnSpeed;

    public float rechargeTime;
    public float bulletSpeed;
    public float bulletRecoil;
    public float bulletLifeSpan;
    public int damage;

    public int health;

    [HideInInspector] public GameObject bullet;
    public Powerup ActivePowerup;

    [SerializeField] public TMP_Text backPedalTxt;

    private TankHealth tankHealth;

    [SerializeField] public Transform indicatorLocation;

    private void Start()
    {
        var gameSettings = FindObjectOfType<GameSettings>();

        transform.position = gameSettings.PlayerSpawns[tankNumber - 1].position;
        transform.rotation = gameSettings.PlayerSpawns[tankNumber - 1].rotation;

        GetComponent<SpriteRenderer>().color = gameSettings.PlayerColors[tankNumber - 1];

        tankHealth = GetComponent<TankHealth>();

        if (GameModeManager.GameModes[GameSettings.Instance.gameMode].IsRandomSpawn == false)
        {
            transform.position = tankHealth.spawn.position;
            transform.rotation = tankHealth.spawn.rotation;
        }
    }

}
