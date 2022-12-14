using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class GameModeManager {

    public static readonly Dictionary<GameModeType, GameMode> GameModes = new Dictionary<GameModeType, GameMode>();
    private static bool IsInitialized { get; set; }

    private static void Initialize()
    {
        IsInitialized = true;

        GameModes.Clear();

        var assembly = Assembly.GetAssembly(typeof(GameMode));

        var allGamemodeTypes = assembly.GetTypes().Where(t => typeof(GameMode).IsAssignableFrom(t) && t.IsAbstract == false);

        foreach (var gamemodeType in allGamemodeTypes)
        {
            if (Activator.CreateInstance(gamemodeType) is GameMode gamemode) GameModes.Add(gamemode.GameModeType, gamemode);
        }
    }

    public static string GameModeToString(GameModeType gameModeType)
    {
        if (!IsInitialized)
            Initialize();

        return GameModes[gameModeType].DisplayName;
    }

    public static GameModeType StringToGameMode(string gameModeName)
    {
        if (!IsInitialized)
            Initialize();

        foreach (GameModeType gameModeType in Enum.GetValues(typeof(GameModeType)))
        {
            GameMode gameMode = GameModes[gameModeType];

            if (gameMode.DisplayName == gameModeName)
            {
                return gameMode.GameModeType;
            }
        }

        Debug.LogError($"Game Mode Type: {gameModeName} not found!");
        throw new Exception($"Game Mode Type: {gameModeName} not found!");
    }

    public static void Start(Tank[] tanks, GameModeType gameModeType)
    {
        if (!IsInitialized)
            Initialize();

        GameModes[gameModeType].Start(tanks);
    }

    public static void Update(Tank[] tanks, GameModeType gameModeType)
    {
        if (!IsInitialized)
            Initialize();

        GameModes[gameModeType].Update(tanks);
    }

    public static bool IsWon(Tank[] tanks, GameModeType gameModeType)
    {
        if (!IsInitialized)
            Initialize();

        return GameModes[gameModeType].IsWon(tanks);
    }

    public static List<ScoreEntry> GetScores(GameModeType gameModeType)
    {
        if (!IsInitialized)
            Initialize();
            
        return GameModes[gameModeType].GenerateScores();
    }

}
