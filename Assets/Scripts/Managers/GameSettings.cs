using UnityEngine;

public class GameSettings : MonoBehaviour {

    [Header("Spawn Places")]
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;
    public Transform[] PlayerSpawns { get; private set; }

    [Header("Tank Colors")]
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color[] PlayerColors { get; private set; }

    [Header("Game Settings")]
    public GameModeType gameMode;
    public bool isWon;

    [Header("UI Settings")]
    public GameObject winScreenUI;

    public static GameSettings Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        gameMode = (GameModeType)PlayerPrefs.GetInt("GameMode");

        PlayerSpawns = new[] { spawn1, spawn2, spawn3, spawn4 };
        PlayerColors = new[] { color1, color2, color3, color4 };

        isWon = false;

        for (var i = 1; i <= 4; i++)
        {
            if (PlayerPrefs.GetInt($"player{i}", 1) != 1) continue;

            if (PlayerPrefs.GetInt($"player{i}AI", 0) == 1)
            {
                SpawnAI(i);
            }
            else
            {
                SpawnPlayer(i);
            }

            var tanks = FindObjectsOfType<Tank>();

            foreach (var tank in tanks)
            {
                if (tank.tankNumber == 0)
                    tank.tankNumber = i;
            }
        }
    }

    private void SpawnPlayer(int tankNumber)
    {
        foreach (TankType tankType in TankManager.TankTypes)
        {
            if (tankType.Name == PlayerPrefs.GetString($"player{tankNumber}tank", "Minitank"))
            {
                Instantiate(
                    tankType.Prefab,
                    PlayerSpawns[tankNumber - 1].position,
                    PlayerSpawns[tankNumber - 1].rotation
                );
            }
        }
    }

    private void SpawnAI(int tankNumber)
    {
        foreach (TankType tankType in TankManager.TankTypes)
        {
            if (tankType.Name == PlayerPrefs.GetString($"player{tankNumber}tank", "Minitank"))
            {
                Instantiate(
                    tankType.PrefabAI,
                    PlayerSpawns[tankNumber - 1].position,
                    PlayerSpawns[tankNumber - 1].rotation
                );
            }
        }
    }

    private void Start()
    {
        if (winScreenUI.activeInHierarchy)
        {
            winScreenUI.SetActive(false);
        }

        GameModeManager.Start(FindObjectsOfType<Tank>(), gameMode);
    }

    private void Update()
    {
        if (isWon)
            return;

        GameModeManager.Update(FindObjectsOfType<Tank>(), gameMode);

        if (GameModeManager.IsWon(FindObjectsOfType<Tank>(), gameMode))
        {
            StopGame();

            isWon = true;
            winScreenUI.SetActive(true);

            if (PlayerPrefs.GetInt("IsSupporting", 0) == 1 && !isWon)
            {
                if (FindObjectOfType<AdManager>() != null)
                {
                    var randNumber = Random.Range(0, 10);

                    if (randNumber <= 2)
                        FindObjectOfType<AdManager>().ShowInterstitialAd();
                }
            }
        }
    }

    private void StopGame()
    {
        var tankMovementControls = FindObjectsOfType<TankMovementControls>();

        foreach (var tankMovementControl in tankMovementControls)
        {
            Destroy(tankMovementControl);
        }

        var tankWeaponControls = FindObjectsOfType<TankWeaponControls>();

        foreach (var tankWeaponControl in tankWeaponControls)
        {
            Destroy(tankWeaponControl);
        }

        var tankMovements = FindObjectsOfType<TankMovement>();

        foreach (var tankMovement in tankMovements)
        {
            tankMovement.keepMoving = false;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

}
