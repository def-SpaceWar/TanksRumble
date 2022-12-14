using UnityEngine;
using UnityEngine.UI;   

public class MapSwitcher : MonoBehaviour {

    [SerializeField] private GameObject scrollBar;
    private float scrollPos;
    private float[] pos;
    public int SelectedMap { get; private set; }

    private void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(
                        scrollBar.GetComponent<Scrollbar>().value,
                        pos[i],
                        0.25f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = new Vector2(1f, 1f);

                SelectedMap = i;

                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = new Vector2(0.7f, 0.7f);
                    }
                }
            }
        }
    }

}
