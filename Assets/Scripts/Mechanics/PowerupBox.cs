using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerupBox : MonoBehaviour {

    private PowerupType m_PowerupType;

    private void Start()
    {
        var values = Enum.GetValues(typeof(PowerupType));
        var random = Random.Range(0f, 100f);

        var i = 0f;

        foreach (int value in values)
        {
            if (random >= i && random <= value + i)
                m_PowerupType = (PowerupType) value;

            i += value;
        }

        Debug.Log(m_PowerupType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Tank>() != null)
        {
            var isUsed = PowerupProcessor.GiveTankPowerup(collision.gameObject.GetComponent<Tank>(), m_PowerupType);

            if (isUsed)
                DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
        Destroy(this);
    }

}
