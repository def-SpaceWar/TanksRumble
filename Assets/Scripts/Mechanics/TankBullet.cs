using UnityEngine;

public class TankBullet : MonoBehaviour {

    public float bulletLifeSpan;

    public int tankNumber;
    public float speed;
    public int damage;

    private Rigidbody2D m_Rb;
    private SpriteRenderer m_SpriteRenderer;
    private bool m_Started;

    [SerializeField] private string audioId;

    private void Awake()
    {
        tankNumber = GetComponentInParent<TankWeapon>().tankNumber;
        speed = GetComponentInParent<TankWeapon>().speed;
        damage = GetComponentInParent<TankWeapon>().damage;
        bulletLifeSpan = GetComponentInParent<TankWeapon>().bulletLifeSpan;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        transform.SetParent(null);

        m_Rb = GetComponent<Rigidbody2D>();

        m_SpriteRenderer.color = FindObjectOfType<GameSettings>().PlayerColors[tankNumber - 1];
        m_Rb.AddRelativeForce(Vector2.right * speed);
    }

    private void Start()
    {
        Invoke(nameof(MakeStarted), Global.WaitToCheckBulletSpeed);
    }

    private void Update()
    {
        if (m_Started)
        {
            Invoke(nameof(DestroySelf), bulletLifeSpan);
        }
    }

    private void MakeStarted()
    {
        m_Started = true;
    }

    private void DestroySelf()
    {
        if (Global.Instance.regExplosion != null)
        {
            Instantiate(Global.Instance.regExplosion, transform.position, transform.rotation);
        }

        Destroy(gameObject);
        Destroy(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TankHealth>() != null)
        {
            TankHealth tankHealth = collision.gameObject.GetComponent<TankHealth>();

            if (tankHealth.tankNumber == tankNumber)
            {
                DestroySelf();
                return;
            }

            if (tankHealth.health > 0)
            {
                if (GameModeManager.GameModes[GameSettings.Instance.gameMode].HasTeams)
                {
                    Tank[] tanks = FindObjectsOfType<Tank>();

                    Tank tank = null;

                    foreach (Tank _tank in tanks)
                    {
                        if (_tank.tankNumber == collision.gameObject.GetComponent<Tank>().tankNumber)
                        {
                            tank = _tank;
                            break;
                        }
                    }

                    Tank thisTank = null;

                    foreach (Tank _tank in tanks)
                    {
                        if (_tank.tankNumber == tankNumber)
                        {
                            thisTank = _tank;
                            break;
                        }
                    }

                    if (thisTank.gameObject.GetComponent<TankTeam>().team == tank.gameObject.GetComponent<TankTeam>().team)
                    {
                        DestroySelf();
                        return;
                    }
                }

                if (tankHealth.health - damage <= 0)
                {
                    TankWeapon[] tankWeapons = FindObjectsOfType<TankWeapon>();

                    foreach (TankWeapon tankWeapon in tankWeapons)
                    {
                        if (tankWeapon.tankNumber == tankNumber)
                        {
                            if (collision.gameObject.GetComponent<Tank>().tankNumber != tankNumber)
                            {
                                tankWeapon.kills += 1;
                            }
                            else
                            {
                                DestroySelf();
                                return;
                            }
                        }
                    }
                }

                collision.gameObject.GetComponent<TankHealth>().health -= damage;
            }
        }
        else if (collision.gameObject.GetComponent<TankTurret>() != null)
        {
            TankHealth tankHealth = collision.gameObject.GetComponent<TankTurret>().GetComponentInParent<TankHealth>();

            if (tankHealth != null)
            {
                if (tankHealth.tankNumber == tankNumber)
                {
                    DestroySelf();
                    return;
                }

                if (tankHealth.health > 0)
                {
                    if (GameModeManager.GameModes[GameSettings.Instance.gameMode].HasTeams)
                    {
                        Tank[] tanks = FindObjectsOfType<Tank>();

                        Tank tank = null;

                        foreach (Tank _tank in tanks)
                        {
                            if (_tank.tankNumber == collision.gameObject.GetComponent<Tank>().tankNumber)
                            {
                                tank = _tank;
                                break;
                            }
                        }

                        Tank thisTank = null;

                        foreach (Tank _tank in tanks)
                        {
                            if (_tank.tankNumber == tankNumber)
                            {
                                thisTank = _tank;
                                break;
                            }
                        }

                        if (thisTank.gameObject.GetComponent<TankTeam>().team == tank.gameObject.GetComponent<TankTeam>().team)
                        {
                            DestroySelf();
                            return;
                        }
                    }

                    if (tankHealth.health - damage <= 0)
                    {
                        TankWeapon[] tankWeapons = FindObjectsOfType<TankWeapon>();

                        foreach (TankWeapon tankWeapon in tankWeapons)
                        {
                            if (tankWeapon.tankNumber != tankNumber)
                            {
                                continue;
                            }

                            if (collision.gameObject.GetComponent<Tank>().tankNumber != tankNumber)
                            {
                                tankWeapon.kills += 1;
                            }
                            else
                            {
                                DestroySelf();
                                return;
                            }
                        }
                    }

                    collision.gameObject.GetComponent<TankHealth>().health -= damage;
                }
            }
        }

        if (collision.gameObject.GetComponent<TankBullet>() != null)
        {
            Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, Global.BulletExplodeRadius);

            foreach (Collider2D objectInRange in objectsInRange)
            {
                if (objectInRange.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    objectInRange.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(m_Rb.velocity * Global.BulletKnockback, transform.position);
                }
            }
        }

        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(m_Rb.velocity * Global.BulletKnockback, transform.position);
        }

        AudioManager.Instance.Play(audioId);
        DestroySelf();
    }

}
