using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Tank))]
public class TankHealth : MonoBehaviour {

    public int tankNumber;
    public int health;

    private Rigidbody2D m_Rb;
    public bool respawning;
    public int Lives { get; private set; } = 10;

    [HideInInspector] public Transform spawn;

    private void Start()
    {
        tankNumber = GetComponent<Tank>().tankNumber;
        health = GetComponent<Tank>().health;

        m_Rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            if (!respawning)
            {
                if (Global.Instance.bigExplosion != null)
                    Instantiate(Global.Instance.bigExplosion, transform.position, transform.rotation);

                // AudioManager.Instance.Play("tank_death");

                if (GameModeManager.GameModes[GameSettings.Instance.gameMode].HasLives)
                    Lives -= 1;

                respawning = true;
                Invoke(nameof(Respawn), 5);

                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r / 2, GetComponent<SpriteRenderer>().color.g / 2, GetComponent<SpriteRenderer>().color.b / 2);
            }
        }
    }

    private void Respawn()
    {
        if (GameModeManager.GameModes[GameSettings.Instance.gameMode].HasLives)
        {
            if (Lives <= 0) return;

            GetComponent<Tank>().backPedalTxt.text = Lives.ToString();
            Invoke(nameof(ClearBackpedal), 1f);
        }

        health = GetComponent<Tank>().health;
        m_Rb.velocity *= 0;
        m_Rb.angularVelocity *= 0;

        if (GameModeManager.GameModes[GameSettings.Instance.gameMode].IsRandomSpawn)
        {
            var random = new Random();
            var randIndex = random.Next(0, GameSettings.Instance.PlayerSpawns.Length);

            transform.position = GameSettings.Instance.PlayerSpawns[randIndex].position;
            transform.rotation = GameSettings.Instance.PlayerSpawns[randIndex].rotation;
        }
        else
        {
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        respawning = false;

        GetComponent<SpriteRenderer>().color = GameSettings.Instance.PlayerColors[tankNumber - 1];
    }

    private void ClearBackpedal()
    {
        GetComponent<Tank>().backPedalTxt.text = "";
    }

}
