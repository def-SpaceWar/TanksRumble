using System.Collections.Generic;

public abstract class GameMode {

    public abstract GameModeType GameModeType { get; }
    public abstract string DisplayName { get; }
    public abstract bool HasLives { get; }
    public abstract bool IsRandomSpawn { get; }
    public abstract bool HasTeams { get; }
    public abstract int MinPlayers { get; }

    public abstract void Start(Tank[] tanks);
    public abstract void Update(Tank[] tanks);
    public abstract bool IsWon(Tank[] tanks);

    public abstract List<ScoreEntry> GenerateScores();

}
