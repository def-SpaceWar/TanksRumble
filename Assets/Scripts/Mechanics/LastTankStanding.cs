using System.Collections.Generic;
using UnityEngine;

public class LastTankStanding : GameMode {

    public override GameModeType GameModeType => GameModeType.LastTankStanding;
    public override string DisplayName => "Last Tank Standing";
    public override bool HasLives => true;
    public override bool IsRandomSpawn => true;
    public override bool HasTeams => false;
    public override int MinPlayers => 2;


    private int totalTankCount;

    public override void Start(Tank[] tanks)
    {
        totalTankCount = tanks.Length;

        return;
    }

    private readonly Dictionary<int, int> deadTanks = new Dictionary<int, int>();
    private int tanksDeadSoFar;

    public override void Update(Tank[] tanks)
    {
        TankWeapon[] tankWeapons = Object.FindObjectsOfType<TankWeapon>();

        foreach (TankWeapon tankWeapon in tankWeapons)
        {
            if (tankWeapon.GetComponent<TankHealth>().Lives <= 0)
            {
                try
                {
                    int score = deadTanks[tankWeapon.tankNumber];
                }
                catch
                {
                    deadTanks.Add(tankWeapon.tankNumber, tanksDeadSoFar);
                    tanksDeadSoFar++;
                }
            }
        }
    }

    private int winner;

    public override bool IsWon(Tank[] tanks)
    {
        var tanksAlive = totalTankCount - tanksDeadSoFar;

        if (tanksAlive == 1)
        {
            foreach (TankWeapon tankWeapon in Object.FindObjectsOfType<TankWeapon>())
            {
                if (tankWeapon.GetComponent<TankHealth>().Lives > 0)
                {
                    winner = tankWeapon.tankNumber;
                    break;
                }
            }

            return true;
        }

        return false;
    }

    public override List<ScoreEntry> GenerateScores()
    {
        List<ScoreEntry> scoreEntries = new List<ScoreEntry>();

        foreach (KeyValuePair<int, int> pair in deadTanks)
        {
            int kills = 0;

            foreach (TankWeapon tankWeapon in Object.FindObjectsOfType<TankWeapon>())
            {
                if (tankWeapon.tankNumber == pair.Key)
                {
                    kills = tankWeapon.kills;
                    break;
                }
            }

            scoreEntries.Add(new ScoreEntry($"Player {pair.Key}", kills));
        }

        int kill = 0;

        foreach (TankWeapon tankWeapon in Object.FindObjectsOfType<TankWeapon>())
        {
            if (tankWeapon.tankNumber == winner)
            {
                kill = tankWeapon.kills;
                break;
            }
        }

        scoreEntries.Add(new ScoreEntry($"Player {winner}", kill));
        scoreEntries.Reverse();

        return scoreEntries;
    }

}
