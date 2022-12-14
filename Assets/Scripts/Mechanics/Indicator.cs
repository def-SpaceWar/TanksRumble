using UnityEngine;

public class Indicator : MonoBehaviour {

    void Start()
    {
        transform.position = GetComponentInParent<Tank>().indicatorLocation.position;

        switch (GetComponentInParent<TankTeam>().team)
        {
            case 1:
                GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0);
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 1f);
                break;
        }
    }

}
