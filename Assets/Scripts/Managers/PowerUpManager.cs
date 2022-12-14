using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

    [SerializeField] private GameObject powerUpBox;
    [SerializeField] private Transform[] powerUpSpawnPoints;
    [SerializeField] private int minWaitTime = 20;
    [SerializeField] private int maxWaitTime = 40;

    private bool m_IsSpawning;

    private void Update()
    {
        if (!m_IsSpawning && !FindObjectOfType<GameSettings>().isWon)
            StartCoroutine(SpawnBox());
    }

    private IEnumerator SpawnBox()
    {
        m_IsSpawning = true;

        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        int index = Random.Range(1, powerUpSpawnPoints.Length) - 1;

        Instantiate(powerUpBox, powerUpSpawnPoints[index].position, powerUpSpawnPoints[index].rotation);

        m_IsSpawning = false;
    }

}
