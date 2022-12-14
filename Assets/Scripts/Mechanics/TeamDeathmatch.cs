using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamDeathmatch : GameMode {

    public override GameModeType GameModeType => GameModeType.TeamDeathmatch;
    public override string DisplayName => "Team Deathmatch";
    public override bool HasLives => false;
    public override bool IsRandomSpawn => true;
    public override bool HasTeams => true;
    public override int MinPlayers => 4;

    public override void Start(Tank[] tanks)
    {
        // Give all the tanks their teams

        List<int> list = new List<int>
        {
            1,
            1,
            2,
            2
        };

        list = list.OrderBy(x => Random.value).ToList();

        foreach (Tank tank in tanks)
        {
            TankTeam team = tank.gameObject.AddComponent<TankTeam>();
            team.team = list[tank.tankNumber - 1];

            Object.Instantiate(Global.Instance.indicator, tank.transform);
        }
    }

    public override void Update(Tank[] tanks)
    {
        // Do nothing
    }

    public override bool IsWon(Tank[] tanks)
    {
        // If one team reaches 20 kills

        List<Tank> teamOne = new List<Tank>();
        List<Tank> teamTwo = new List<Tank>();

        foreach (Tank tank in tanks)
        {
            switch (tank.GetComponent<TankTeam>().team)
            {
                case 1:
                    teamOne.Add(tank);
                    break;
                case 2:
                    teamTwo.Add(tank);
                    break;
            }
        }

        int teamOneKillz = 0;

        foreach (Tank tank in teamOne)
        {
            teamOneKillz += tank.GetComponent<TankWeapon>().kills;
        }

        if (teamOneKillz >= 20)
        {
            return true;
        }

        int teamTwoKillz = 0;

        foreach (Tank tank in teamTwo)
        {
            teamTwoKillz += tank.GetComponent<TankWeapon>().kills;
        }

        if (teamTwoKillz >= 20)
        {
            return true;
        }

        return false;
    }

    public override List<ScoreEntry> GenerateScores()
    {
        List<Tank> teamOne = new List<Tank>();
        List<Tank> teamTwo = new List<Tank>();

        foreach (Tank tank in Object.FindObjectsOfType<Tank>())
        {
            switch (tank.GetComponent<TankTeam>().team)
            {
                case 1:
                    teamOne.Add(tank);
                    break;
                case 2:
                    teamTwo.Add(tank);
                    break;
            }
        }

        int teamOneKillz = 0;

        foreach (Tank tank in teamOne)
        {
            teamOneKillz += tank.GetComponent<TankWeapon>().kills;
        }

        int teamTwoKillz = 0;

        foreach (Tank tank in teamTwo)
        {
            teamTwoKillz += tank.GetComponent<TankWeapon>().kills;
        }

        List<ScoreEntry> scoreEntries = new List<ScoreEntry>();

        switch (teamOneKillz > teamTwoKillz)
        {
            case true:
                scoreEntries.Add(new ScoreEntry($"Team 1", teamOneKillz));
                scoreEntries.Add(new ScoreEntry($"Team 2", teamTwoKillz));
                break;
            case false:
                scoreEntries.Add(new ScoreEntry($"Team 2", teamTwoKillz));
                scoreEntries.Add(new ScoreEntry($"Team 1", teamOneKillz));
                break;
        }

        // The teams and the winner is the team with the most kills
        return scoreEntries;
    }

}
