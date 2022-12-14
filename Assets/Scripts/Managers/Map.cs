using System;
using UnityEngine;

[Serializable]
public class Map {

    [SerializeField] private Sprite _thumbnail;
    [SerializeField] private int _sceneIndex;
    [SerializeField] private string _name;
    [SerializeField] private Color _textColor;

    public Sprite Thumbnail => _thumbnail;
    public int SceneIndex => _sceneIndex;
    public string Name => _name;
    public Color TextColor => _textColor;

    [SerializeField] private string[] _gameModes;
    public string[] GameModes => _gameModes;

}
