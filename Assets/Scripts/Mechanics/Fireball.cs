using System.Collections;
using UnityEngine;

public class Fireball : Powerup {

    private GameObject Projectile { get; }

    public Fireball()
    {
        Projectile = Global.Instance.fireball;
    }

    public override PowerupType PowerupType => PowerupType.Fireball;
    public override int Duration => 20;

    public override IEnumerator ProcessOnTank(Tank tank)
    {
        var tankWeapon = tank.GetComponent<TankWeapon>();

        tank.ActivePowerup = this;
        tankWeapon.bullet = Projectile;
        tankWeapon.bulletLifeSpan = 5;

        var pos = tankWeapon.shootPoint.localPosition;
        pos.x += 0.25f;
        tankWeapon.shootPoint.transform.localPosition = pos;

        tank.StartCoroutine(CountDown(tank, Duration));
        yield return new WaitForSeconds(Duration);

        tankWeapon.ReInitVars();
        tank.ActivePowerup = null;
    }

}
