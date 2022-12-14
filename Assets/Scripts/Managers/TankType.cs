using System;
using UnityEngine;

[Serializable]
public class TankType {

    [SerializeField] private Sprite _thumbnail;
    [SerializeField] private string _name;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _prefabAI;

    public Sprite Thumbnail => _thumbnail;
    public string Name => _name;
    public GameObject Prefab => _prefab;
    public GameObject PrefabAI => _prefabAI;

}
