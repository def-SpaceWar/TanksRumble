using System.Collections;
using UnityEngine;

public class Invincibility : Powerup {

    public override PowerupType PowerupType => PowerupType.Invincibility;
    public override int Duration => 10;

    public override IEnumerator ProcessOnTank(Tank tank)
    {
        tank.ActivePowerup = this;

        var tankColor = Object.FindObjectOfType<GameSettings>().PlayerColors[tank.tankNumber - 1];
        var tankHealth = tank.GetComponent<TankHealth>();
        var beforeHealth = tankHealth.health;

        tankHealth.health = int.MaxValue;

        tank.StartCoroutine(CountDown(tank, Duration));

        float duration = Duration;
        var white = true;
        var spriteRenderer = tank.GetComponent<SpriteRenderer>();

        const float flashAmount = 20f;

        while (duration > 0)
        {
            spriteRenderer.color = white ? Color.white : tankColor;

            white = !white;

            duration -= Duration / flashAmount;
            yield return new WaitForSeconds(Duration / flashAmount);
        }

        tankHealth.health = beforeHealth;

        tank.ActivePowerup = null;
    }

}
