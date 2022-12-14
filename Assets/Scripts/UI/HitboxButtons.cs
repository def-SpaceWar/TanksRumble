using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HitboxButtons : MonoBehaviour {

    private void Start()
    {
        // Everything with alpha under 0.1 is counted as transparent.
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

}
