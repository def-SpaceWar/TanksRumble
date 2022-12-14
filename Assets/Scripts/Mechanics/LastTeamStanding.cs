using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LastTeamStanding : GameMode {

    public override GameModeType GameModeType => GameModeType.LastTeamStanding;
    public override string DisplayName => "Last Team Standing";
    public override bool HasLives => true;
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
        //
    }

    public override bool IsWon(Tank[] tanks)
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

        int teamOneAlive = 0;
        int teamOneKillz = 0;

        foreach (Tank tank in teamOne)
        {
            if (tank.GetComponent<TankHealth>().Lives > 0)
            {
                teamOneAlive += 1;
            }

            teamOneKillz += tank.GetComponent<TankWeapon>().kills;
        }

        int teamTwoAlive = 0;
        int teamTwoKillz = 0;

        foreach (Tank tank in teamTwo)
        {
            if (tank.GetComponent<TankHealth>().Lives > 0)
            {
                teamTwoAlive += 1;
            }

            teamTwoKillz += tank.GetComponent<TankWeapon>().kills;
        }

        if (teamOneAlive == 0)
        {
            return true;
        }

        if (teamTwoAlive == 0)
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

        int teamOneAlive = 0;
        int teamOneKillz = 0;

        foreach (Tank tank in teamOne)
        {
            if (tank.GetComponent<TankHealth>().Lives > 0)
            {
                teamOneAlive += 1;
            }

            teamOneKillz += tank.GetComponent<TankWeapon>().kills;
        }

        int teamTwoAlive = 0;
        int teamTwoKillz = 0;

        foreach (Tank tank in teamTwo)
        {
            if (tank.GetComponent<TankHealth>().Lives > 0)
            {
                teamTwoAlive += 1;
            }

            teamTwoKillz += tank.GetComponent<TankWeapon>().kills;
        }

        List<ScoreEntry> scoreEntries = new List<ScoreEntry>();

        if (teamOneAlive > teamTwoAlive)
        {
            scoreEntries.Add(new ScoreEntry($"Team 1", teamOneKillz));
            scoreEntries.Add(new ScoreEntry($"Team 2", teamTwoKillz));
        }
        else
        {
            scoreEntries.Add(new ScoreEntry($"Team 2", teamTwoKillz));
            scoreEntries.Add(new ScoreEntry($"Team 1", teamOneKillz));
        }

        // The teams and the winner is the team with the most kills
        return scoreEntries;
    }

}
