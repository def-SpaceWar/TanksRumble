using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;

    private List<ScoreEntry> scoreEntries;
    private List<Transform> scoreTransforms = new List<Transform>();

    private bool madeScore;

    private void Update()
    {
        if (GameSettings.Instance.isWon && !madeScore)
        {
            madeScore = true;
            MakeScreen();
        }
    }

    private void MakeScreen()
    {
        entryTemplate.gameObject.SetActive(false);

        scoreEntries = GameModeManager.GetScores(GameSettings.Instance.gameMode);

        foreach (ScoreEntry scoreEntry in scoreEntries)
        {
            CreateScoreEntry(scoreEntry);
        }

        /*
        // if the game mode is a team game mode it should show the teams instead of the players!
        if (GameModeManager.GameModes[FindObjectOfType<GameSettings>().gameMode].HasTeams)
        {
            // team score logic

            // group all tanks by a team and add up all their scores collectively.
        }
        else
        {
            // ffa score logic

            // every tank/person has their own score!
        }
        */
    }

    private void CreateScoreEntry(ScoreEntry scoreEntry)
    {
        const float TEMPLATE_HEIGHT = 100f;

        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -TEMPLATE_HEIGHT * scoreTransforms.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = scoreTransforms.Count + 1;
        string rankString;

        switch (rank)
        {
            case 1:
                rankString = $"1st";
                break;
            case 2:
                rankString = $"2nd";
                break;
            case 3:
                rankString = $"3rd";
                break;
            default:
                rankString = $"{rank}th";
                break;
        }

        entryTransform.Find("Place").GetComponent<Text>().text = rankString;
        entryTransform.Find("Name").GetComponent<Text>().text = scoreEntry.name;
        entryTransform.Find("Score").GetComponent<Text>().text = scoreEntry.score.ToString();

        switch (scoreEntry.name)
        {
            case "Player 1":
                ReplaceIn(entryTransform, GameSettings.Instance.PlayerColors[0]);
                break;
            case "Player 2":
                ReplaceIn(entryTransform, GameSettings.Instance.PlayerColors[1]);
                break;
            case "Player 3":
                ReplaceIn(entryTransform, GameSettings.Instance.PlayerColors[2]);
                break;
            case "Player 4":
                ReplaceIn(entryTransform, GameSettings.Instance.PlayerColors[3]);
                break;
            case "Juggernaut":
                ReplaceIn(entryTransform, new Color(0.5f, 0, 1));
                break;
            case "Team 1":
                ReplaceIn(entryTransform, new Color(1f, 0, 0));
                break;
            case "Team 2":
                ReplaceIn(entryTransform, new Color(0, 0, 1f));
                break;
            default:
                ReplaceIn(entryTransform, Color.black);
                break;
        }

        scoreTransforms.Add(entryTransform);
    }

    private void ReplaceIn(Transform entryTransform, Color color)
    {
        entryTransform.Find("Place").GetComponent<Text>().color = color;
        entryTransform.Find("Name").GetComponent<Text>().color = color;
        entryTransform.Find("Score").GetComponent<Text>().color = color;
    }

}
