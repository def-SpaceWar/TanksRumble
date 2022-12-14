using System.Collections;
using UnityEngine;

public class Speed : Powerup {

    public override PowerupType PowerupType => PowerupType.Speed;
    public override int Duration => 15;

    public override IEnumerator ProcessOnTank(Tank tank)
    {
        tank.ActivePowerup = this;
        tank.GetComponent<TankMovement>().movePower *= 1.25f;
        tank.GetComponent<TankWeapon>().rechargeTime /= 2;
        tank.GetComponent<TankWeapon>().recoilPower *= 1.1f;
        tank.GetComponent<TankWeapon>().speed *= 1.5f;

        tank.StartCoroutine(CountDown(tank, Duration));
        yield return new WaitForSeconds(Duration);

        tank.GetComponent<TankMovement>().ReInitVars();
        tank.GetComponent<TankWeapon>().ReInitVars();
        tank.ActivePowerup = null;
    }

}
