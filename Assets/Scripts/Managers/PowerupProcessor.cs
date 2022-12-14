using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class PowerupProcessor {

    private static readonly Dictionary<PowerupType, Powerup> Powerups = new Dictionary<PowerupType, Powerup>();
    private static bool IsInitialized { get; set; }

    private static void Initialize()
    {
        IsInitialized = true;

        Powerups.Clear();

        var assembly = Assembly.GetAssembly(typeof(Powerup));

        var allPowerupTypes = assembly.GetTypes().Where(t => typeof(Powerup).IsAssignableFrom(t) && t.IsAbstract == false);

        foreach (var powerupType in allPowerupTypes)
        {
            var powerup = Activator.CreateInstance(powerupType) as Powerup;
            Powerups.Add(powerup.PowerupType, powerup);
        }
    }

    public static bool GiveTankPowerup(Tank target, PowerupType powerupType)
    {
        if (!IsInitialized)
            Initialize();

        var powerup = Powerups[powerupType];

        if (target.ActivePowerup != null)
            return false;

        if (target.GetComponent<TankHealth>().Lives == 0 && GameModeManager.GameModes[(GameModeType)PlayerPrefs.GetInt("GameMode", 0)].HasLives)
            return false;

        target.StartCoroutine(powerup.ProcessOnTank(target));
        return true;
    }

}
