using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TankTurret : MonoBehaviour {

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = FindObjectOfType<GameSettings>().PlayerColors[GetComponentInParent<Tank>().tankNumber - 1];
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().color = GetComponentInParent<Tank>().GetComponent<SpriteRenderer>().color;
    }

}
