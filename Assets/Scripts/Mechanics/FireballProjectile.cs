using UnityEngine;

public class FireballProjectile : MonoBehaviour {

    private float m_BulletLifeSpan;

    private int m_TankNumber;
    private float m_Speed;
    private int m_Damage;

    private Rigidbody2D m_Rb;
    private bool m_Started;

    private float decreaseRate = 1f; // The rate at which a fireball shrinks
    private bool m_IsDecreasing;

    private float m_FireBallKnockBack = 10;

    private void Awake()
    {
        m_TankNumber = GetComponentInParent<TankWeapon>().tankNumber;
        m_Speed = GetComponentInParent<TankWeapon>().speed * 20;
        m_Damage = GetComponentInParent<TankWeapon>().damage;
        m_BulletLifeSpan = GetComponentInParent<TankWeapon>().bulletLifeSpan;

        transform.rotation = GetComponentInParent<Transform>().rotation;

        transform.SetParent(null);

        m_Rb = GetComponent<Rigidbody2D>();

        m_Rb.AddRelativeForce(Vector2.right * m_Speed);
    }

    private void Start()
    {
        Invoke(nameof(MakeStarted), Global.WaitToCheckBulletSpeed);
    }

    private void Update()
    {
        if (m_Started)
        {
            Invoke(nameof(StartDecreasing), m_BulletLifeSpan);
            m_Started = false;
        }
    }

    private void FixedUpdate()
    {
        if (!m_IsDecreasing)
            return;

        var newScale = transform.localScale;
        newScale.x -= decreaseRate * Time.fixedDeltaTime;
        newScale.y -= decreaseRate * Time.fixedDeltaTime;

        var rb = GetComponent<Rigidbody2D>();
        rb.mass += decreaseRate * Time.fixedDeltaTime * 2;

        if (newScale.x <= 0 || newScale.y <= 0)
        {
            DestroySelf();
        }
        else
        {
            transform.localScale = newScale;
        }
    }

    private void MakeStarted()
    {
        m_Started = true;
    }

    private void StartDecreasing()
    {
        m_IsDecreasing = true;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TankHealth>() != null)
        {
            if (collision.gameObject.GetComponent<TankHealth>().health > 0)
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
                        if (_tank.tankNumber == m_TankNumber)
                        {
                            thisTank = _tank;
                            break;
                        }
                    }

                    if (thisTank.gameObject.GetComponent<TankTeam>().team == tank.gameObject.GetComponent<TankTeam>().team)
                    {
                        return;
                    }
                }

                if (collision.gameObject.GetComponent<TankHealth>().health - m_Damage <= 0)
                {
                    var tankWeapons = FindObjectsOfType<TankWeapon>();

                    foreach (var tankWeapon in tankWeapons)
                    {
                        if (tankWeapon.tankNumber == m_TankNumber)
                        {
                            if (collision.gameObject.GetComponent<Tank>().tankNumber != m_TankNumber)
                            {
                                tankWeapon.kills += 1;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }

                collision.gameObject.GetComponent<TankHealth>().health -= m_Damage;
                collision.gameObject.GetComponent<Rigidbody2D>()?.AddForceAtPosition(m_Rb.velocity * Global.BulletKnockback * m_FireBallKnockBack, transform.position);
            }
        }
    }

}
