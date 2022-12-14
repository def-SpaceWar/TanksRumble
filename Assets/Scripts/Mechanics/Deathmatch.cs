using System.Collections.Generic;
using UnityEngine;

public class Deathmatch : GameMode {

    public override GameModeType GameModeType => GameModeType.Deathmatch;
    public override string DisplayName => "Deathmatch";
    public override bool HasLives => false;
    public override bool IsRandomSpawn => true;
    public override bool HasTeams => false;
    public override int MinPlayers => 2;

    public override void Start(Tank[] tanks)
    {
        return;
    }

    public override void Update(Tank[] tanks)
    {
        return;
    }

    public override bool IsWon(Tank[] tanks)
    {
        foreach (var tank in tanks)
        {
            var tankWeapon = tank.GetComponent<TankWeapon>();

            if (tankWeapon.kills >= 10)
            {
                return true;
            }
        }

        return false;
    }

    public override List<ScoreEntry> GenerateScores()
    {
        List<ScoreEntry> scoreEntries = new List<ScoreEntry>();

        Dictionary<int, int> playerNumberToKills = new Dictionary<int, int>();

        foreach (TankWeapon tankWeapon in Object.FindObjectsOfType<TankWeapon>())
        {
            playerNumberToKills[tankWeapon.tankNumber] = tankWeapon.kills;
        }

        foreach (KeyValuePair<int, int> pair in playerNumberToKills)
        {
            scoreEntries.Add(new ScoreEntry($"Player {pair.Key}", pair.Value));
        }

        for (int i = 0; i < scoreEntries.Count; i++)
        {
            for (int j = i + 1; j < scoreEntries.Count; j++)
            {
                if (scoreEntries[j].score > scoreEntries[i].score)
                {
                    ScoreEntry tmp = scoreEntries[i];
                    scoreEntries[i] = scoreEntries[j];
                    scoreEntries[j] = tmp;
                }
            }
        }

        return scoreEntries;
    }

}
