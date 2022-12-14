using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeSwitcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private GameModeType _gameMode;
    [SerializeField] private Text text;
    private List<GameModeType> _gameModes;
    private Image image;

    private int prevMapIdx;

    private void Start()
    {
        image = GetComponent<Image>();

        _gameModes = new List<GameModeType>();

        foreach (string gameMode in FindObjectOfType<MapManager>().maps[FindObjectOfType<MapSwitcher>().SelectedMap].GameModes)
        {
            _gameModes.Add(GameModeManager.StringToGameMode(gameMode));
        }

        _gameMode = _gameModes[0];
        PlayerPrefs.SetInt("GameMode", (int)_gameMode);
        image.color = FindObjectOfType<MapManager>().maps[FindObjectOfType<MapSwitcher>().SelectedMap].TextColor;
        text.color = image.color;

        prevMapIdx = FindObjectOfType<MapSwitcher>().SelectedMap;
    }

    private void Update()
    {
        if (FindObjectOfType<MapSwitcher>() == null) return;

        if (FindObjectOfType<MapSwitcher>().SelectedMap != prevMapIdx)
        {
            _gameModes = new List<GameModeType>();

            foreach(string gameMode in FindObjectOfType<MapManager>().maps[FindObjectOfType<MapSwitcher>().SelectedMap].GameModes)
            {
                _gameModes.Add(GameModeManager.StringToGameMode(gameMode));
            }

            _gameMode = _gameModes[0];
            PlayerPrefs.SetInt("GameMode", (int)_gameMode);
            image.color = FindObjectOfType<MapManager>().maps[FindObjectOfType<MapSwitcher>().SelectedMap].TextColor;
            text.color = image.color;

            prevMapIdx = FindObjectOfType<MapSwitcher>().SelectedMap;
        }

        text.text = GameModeManager.GameModeToString(_gameMode);
        prevMapIdx = FindObjectOfType<MapSwitcher>().SelectedMap;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // cycle through gamemodes
        try
        {
            _gameMode = _gameModes[_gameModes.IndexOf(_gameMode) + 1];
        }
        catch
        {
            _gameMode = _gameModes[0];
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // set gamemode
        PlayerPrefs.SetInt("GameMode", (int)_gameMode);
    }

}
