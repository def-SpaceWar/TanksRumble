using UnityEngine;

// holds values for Juggernaut gamemode
public class JuggernautConfig : MonoBehaviour {

    [SerializeField] private Transform _JuggernautSpawn;
    public Transform JuggernautSpawn => _JuggernautSpawn;

    [SerializeField] private Transform spawn1;
    [SerializeField] private Transform spawn2;
    [SerializeField] private Transform spawn3;

    public Transform[] PlayerSpawns { get; private set; }

    void Start()
    {
        PlayerSpawns = new[] { spawn1, spawn2, spawn3 };
    }

}
