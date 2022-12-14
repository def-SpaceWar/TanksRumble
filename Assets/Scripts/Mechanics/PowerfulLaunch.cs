using System.Collections;
using UnityEngine;

public class PowerfulLaunch : Powerup {

    public override PowerupType PowerupType => PowerupType.PowerfulLaunch;
    public override int Duration => 15;

    public override IEnumerator ProcessOnTank(Tank tank)
    {
        tank.ActivePowerup = this;
        tank.GetComponent<TankMovement>().movePower *= .5f;
        tank.GetComponent<TankWeapon>().rechargeTime *= 2;
        tank.GetComponent<TankWeapon>().recoilPower *= 1.5f;
        tank.GetComponent<TankWeapon>().speed *= 20f; // the bullet goes insanely fast

        tank.StartCoroutine(CountDown(tank, Duration));
        float duration = Duration;

        const float distance = 20;

        tank.gameObject.AddComponent<LineRenderer>();

        var lineRenderer = tank.GetComponent<LineRenderer>();

        lineRenderer.startColor = new Color(1, 0, 0);
        lineRenderer.endColor = new Color(0, 0, 0);

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.3f;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(0, 0, 0));

        while (duration > 0f)
        {
            var hitInfo = Physics2D.Raycast(tank.GetComponent<TankWeapon>().shootPoint.position, tank.transform.right, distance);

            if (hitInfo)
            {
                // Debug.DrawLine(tank.GetComponent<TankWeapon>().shootPoint.position, hitInfo.point, Color.red);
                lineRenderer.SetPosition(0, tank.transform.position);
                lineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                //Debug.DrawLine(tank.GetComponent<TankWeapon>().shootPoint.position, tank.transform.position + tank.transform.right * distance, Color.green);
                lineRenderer.SetPosition(0, tank.transform.position);
                lineRenderer.SetPosition(1, tank.transform.position + tank.transform.right * distance);
            }

            duration -= Time.deltaTime;
            yield return null;
        }

        Object.Destroy(lineRenderer);

        tank.GetComponent<TankMovement>().ReInitVars();
        tank.GetComponent<TankWeapon>().ReInitVars();
        tank.ActivePowerup = null;
    }

}
