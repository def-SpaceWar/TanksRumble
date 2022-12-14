using System.Collections.Generic;
using UnityEngine;

public class JuggernautMode : GameMode
{
        public override GameModeType GameModeType => GameModeType.Juggernaut;
        public override string DisplayName => "Juggernaut";
        public override bool HasLives => true;
        public override bool IsRandomSpawn => false;
        public override bool HasTeams => true;
        public override int MinPlayers => 4;

        public override void Start(Tank[] tanks)
        {
                int randomTank = Random.Range(0, tanks.Length - 1); // This tank will become juggernaut!

                TankTeam[] tankTeams = new TankTeam[tanks.Length];

                for (int a = 0; a < tanks.Length; a++)
                {
                        tankTeams.SetValue(tanks[a].gameObject.AddComponent<TankTeam>(), a);
                }

                foreach (TankTeam tankTeam in tankTeams)
                {
                        tankTeam.team = 1;
                }

                tankTeams[randomTank].team = 2; // Juggernaut Team!
                tanks[randomTank].transform.localScale *= 1.5f;
                tanks[randomTank].GetComponent<TankWeapon>().bulletSize = 1.2f;
                tanks[randomTank].GetComponent<Tank>().movePower *= 9;
                tanks[randomTank].GetComponent<Tank>().bulletSpeed *= 9;

                tanks[randomTank].GetComponent<TankMovement>().ReInitVars();

                // Set random spawns
                int i = 0;

                foreach (Tank tank in tanks)
                {
                        if (tank.GetComponent<TankTeam>().team == 2)
                        {
                                tank.GetComponent<TankHealth>().spawn = Object.FindObjectOfType<JuggernautConfig>().JuggernautSpawn;
                        }
                        else
                        {
                                tank.GetComponent<TankHealth>().spawn = Object.FindObjectOfType<JuggernautConfig>().PlayerSpawns[i];
                                i++;
                        }

                        tank.transform.position = tank.GetComponent<TankHealth>().spawn.position;
                        tank.transform.rotation = tank.GetComponent<TankHealth>().spawn.rotation;
                }
        }

        public override void Update(Tank[] tanks)
        {
                // TODO: If the juggernaut goes below a certain amount of lives, BUFF HIM!!!

                return;
        }

        private int winningTeam;

        public override bool IsWon(Tank[] tanks)
        {
                List<TankTeam> teamOne = new List<TankTeam>();
                List<TankTeam> teamTwo = new List<TankTeam>();

                TankHealth[] tankHealths = Object.FindObjectsOfType<TankHealth>();

                foreach (TankHealth tankHealth in tankHealths)
                {
                        if (tankHealth.Lives > 0) continue;

                        if (tankHealth.GetComponent<TankTeam>().team == 1)
                        {
                                teamOne.Add(tankHealth.GetComponent<TankTeam>());
                        }
                        else if (tankHealth.GetComponent<TankTeam>().team == 2)
                        {
                                teamTwo.Add(tankHealth.GetComponent<TankTeam>());
                        }
                }

                if (teamOne.Count == 0 && teamTwo.Count > 0)
                {
                        winningTeam = 2;
                        return true;
                }

                if (teamOne.Count > 0 && teamTwo.Count == 0)
                {
                        winningTeam = 1;
                        return true;
                }

                return false;
        }

        public override List<ScoreEntry> GenerateScores()
        {
                List<ScoreEntry> scoreEntries = new List<ScoreEntry>();

                int juggernautKills = 0;
                int humanKills = 0;

                foreach (TankWeapon tankWeapon in Object.FindObjectsOfType<TankWeapon>())
                {
                        if (tankWeapon.GetComponent<TankTeam>().team == 1) humanKills += tankWeapon.kills;
                        else if (tankWeapon.GetComponent<TankTeam>().team == 2) juggernautKills += tankWeapon.kills;
                }

                if (winningTeam == 1)
                {
                        scoreEntries.Add(new ScoreEntry("Humans", humanKills));
                        scoreEntries.Add(new ScoreEntry("Juggernaut", juggernautKills));
                }

                if (winningTeam == 2)
                {
                        scoreEntries.Add(new ScoreEntry("Juggernaut", juggernautKills));
                        scoreEntries.Add(new ScoreEntry("Humans", humanKills));
                }

                return scoreEntries;
        }
}
