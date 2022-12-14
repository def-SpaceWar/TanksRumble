using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {

    [SerializeField] public Map[] maps;

    // card template
    [SerializeField] private GameObject mapCard;

    // place to put the cards
    [SerializeField] private GameObject contentView;

    private void Start()
    {
        foreach (Map map in maps)
        {
            // Make all the cards for the 'MapSwitcher'!
            GameObject card = Instantiate(mapCard, contentView.transform);

            // modify the card!
            card.GetComponent<Image>().sprite = map.Thumbnail;
            card.GetComponentInChildren<Text>().text = map.Name;
            card.GetComponentInChildren<Text>().color = map.TextColor;
        }
    }

}
