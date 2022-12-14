using System.Collections;
using UnityEngine;

public abstract class Powerup {

    public abstract PowerupType PowerupType { get; }
    public abstract int Duration { get; }

    private protected static IEnumerator CountDown(Tank tank, int secs)
    {
        var secsLeft = secs;

        tank.backPedalTxt.text = secsLeft.ToString();

        while (true)
        {
            if (secsLeft == 0)
                break;

            yield return new WaitForSeconds(1);

            secsLeft -= 1;
            tank.backPedalTxt.text = secsLeft.ToString();
        }

        tank.backPedalTxt.text = "";
    }

    public abstract IEnumerator ProcessOnTank(Tank tank);

}
