using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class TankWeapon : MonoBehaviour {

    public int tankNumber;
    public float rechargeTime;
    public int damage;
    public float speed;
    public float recoilPower;

    public Transform shootPoint;
    private Vector3 m_RealShootPoint;
    public GameObject bullet;

    public bool shootable;
    public bool jammed;

    public int kills;
    public float bulletLifeSpan;

    // So we can increase bullet size for juggernaut mode!
    [HideInInspector] public float bulletSize = 1;

    private string tankType;

    private void Start()
    {
        tankNumber = GetComponent<Tank>().tankNumber;
        StartCoroutine(InitProperties());
        tankType = PlayerPrefs.GetString($"player{tankNumber}tank", "Minitank").ToLower();
        Debug.Log($"{tankNumber}, {tankType}");
    }

    private IEnumerator InitProperties()
    {
        shootable = true;
        jammed = false;
        kills = 0;

        yield return new WaitForSeconds(Global.WaitToSetTankNumber);

        m_RealShootPoint = shootPoint.localPosition;
        GetComponent<Tank>().bullet = bullet;
        tankNumber = GetComponent<Tank>().tankNumber;
        ReInitVars();

        yield return new WaitForSeconds(Global.WaitToInitControls);

        foreach (var weaponControls in FindObjectsOfType<TankWeaponControls>())
        {
            if (weaponControls.tankNumber != tankNumber) continue;
            weaponControls.tankWeapon = this;
            weaponControls.Initialize();
        }
    }

    public void ReInitVars()
    {
        rechargeTime = GetComponent<Tank>().rechargeTime;
        damage = GetComponent<Tank>().damage;
        speed = GetComponent<Tank>().bulletSpeed;
        recoilPower = GetComponent<Tank>().bulletRecoil;
        bulletLifeSpan = GetComponent<Tank>().bulletLifeSpan;
        bullet = GetComponent<Tank>().bullet;
        shootPoint.localPosition = m_RealShootPoint;
    }

    public void Shoot()
    {
        if (jammed)
            return;

        if (shootable)
        {
            if (Global.Instance.smallExplosion != null)
                Instantiate(Global.Instance.smallExplosion, shootPoint.position, shootPoint.rotation);

            GameObject myBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation, transform);
            myBullet.transform.localScale *= bulletSize;

            AudioManager.Instance.Play($"{tankType}_shoot");

            shootable = false;

            if (GetComponent<Rigidbody2D>() != null)
                GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * recoilPower);

            Invoke(nameof(MakeShootable), rechargeTime);
        }
    }

    private void MakeShootable()
    {
        shootable = true;
    }

    private void Update()
    {
        if (GetComponent<TankHealth>() != null)
        {
            jammed = GetComponent<TankHealth>().health <= 0;
        }

        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetBool("IsLoaded", shootable);
        }
    }

}
