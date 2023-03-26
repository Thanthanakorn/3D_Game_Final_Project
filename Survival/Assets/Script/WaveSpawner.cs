using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


public class WaveSpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<GameObject> enemyRemaining;
    public Transform spawnPoint;

    public int currentWave;
    public float timeBetweenWaves = 5;
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 150f;
    public float colliderCheckRadius = 5f;
    public float minDistanceFromOtherEnemies = 150f;
    
    public PlayerStats playerStats;
    
    public TextMeshProUGUI waveText;



    private bool _firstTime = true;
    private Vector3 _firstPosition;

    void Start()
    {
        StartCoroutine(WaitBeforeTheNextRound());
        _firstPosition = spawnPoint.position;
    }

    void Update()
    {
        if (enemyRemaining.All(x => x == null) && _firstTime == false)
        {
            StartCoroutine(WaitBeforeTheNextRound());
        }
    }

    Vector3 GetRandomPosition(float minDistance, float maxDistance, float minDistanceFromOtherEnemies, float colliderCheckRadius)
    {
        Vector3 randomPosition = spawnPoint.position;

        for (int i = 0; i < 100; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere;
            randomDirection.y = 0;
            randomPosition = spawnPoint.position + randomDirection.normalized * UnityEngine.Random.Range(minDistance, maxDistance);

            Collider[] colliders = Physics.OverlapSphere(randomPosition, colliderCheckRadius);

            if (colliders.Length == 0)
            {
                bool isTooCloseToOtherEnemies = false;

                foreach (GameObject enemy in enemyRemaining)
                {
                    if (enemy != null && Vector3.Distance(randomPosition, enemy.transform.position) < minDistanceFromOtherEnemies)
                    {
                        isTooCloseToOtherEnemies = true;
                        break;
                    }
                }

                if (!isTooCloseToOtherEnemies)
                {
                    break;
                }
            }
        }

        return randomPosition;
    }





    void StartNextWave()
    {
        if (currentWave % 4 == 0)
        {
            StartCoroutine(IncreaseEnemies());
        }
        else
        {
            StartCoroutine(IncreaseEnemiesBasedOnPreviousWave());
        }
    }




    private IEnumerator WaitBeforeTheNextRound()
    {
        _firstTime = true;
        yield return new WaitForSecondsRealtime(timeBetweenWaves);
        enemyRemaining.Clear();
        currentWave++;
        waveText.text = "Wave " + currentWave.ToString();
        waveText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        waveText.gameObject.SetActive(false);
        transform.position = _firstPosition;
        if (currentWave > 1)
        {
            playerStats.LevelUp();
        }

        StartNextWave();
    }



    private IEnumerator IncreaseEnemies()
    {
        int sets = (currentWave / 4) + 1;
        for (int i = 0; i < sets; i++)
        {
            foreach (GameObject enemyPrefab in enemyPrefabs)
            {
                Vector3 spawnPosition = GetRandomPosition(minSpawnDistance, maxSpawnDistance, minDistanceFromOtherEnemies, colliderCheckRadius);

                if (_firstTime)
                {
                    GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);
                    if (currentWave > 1)
                    {
                        enemy.GetComponent<EnemyStats>().healthLevel *= Mathf.Pow(1.5f, currentWave - 1);
                        enemy.GetComponent<EnemyStats>().attackLevel *= Mathf.Pow(1.5f, currentWave - 1);
                    }
                    enemyRemaining.Add(enemy);
                    _firstTime = false;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(3f);
                    GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);
                    if (currentWave > 1)
                    {
                        enemy.GetComponent<EnemyStats>().healthLevel *= Mathf.Pow(1.5f, currentWave - 1);
                        enemy.GetComponent<EnemyStats>().attackLevel *= Mathf.Pow(1.5f, currentWave - 1);
                    }
                    enemyRemaining.Add(enemy);
                }
            }
        }
    }

    private IEnumerator IncreaseEnemiesBasedOnPreviousWave()
    {
        int sets = ((currentWave - 1) / 4) + 1;
        for (int i = 0; i < sets; i++)
        {
            foreach (GameObject enemyPrefab in enemyPrefabs)
            {
                Vector3 spawnPosition = GetRandomPosition(minSpawnDistance, maxSpawnDistance, minDistanceFromOtherEnemies, colliderCheckRadius);

                if (_firstTime)
                {
                    GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);
                    if (currentWave > 1)
                    {
                        enemy.GetComponent<EnemyStats>().healthLevel *= Mathf.Pow(1.5f, currentWave - 1);
                        enemy.GetComponent<EnemyStats>().attackLevel *= Mathf.Pow(1.5f, currentWave - 1);
                    }
                    enemyRemaining.Add(enemy);
                    _firstTime = false;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(3f);
                    GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);
                    if (currentWave > 1)
                    {
                        enemy.GetComponent<EnemyStats>().healthLevel *= Mathf.Pow(1.5f, currentWave - 1);
                        enemy.GetComponent<EnemyStats>().attackLevel *= Mathf.Pow(1.5f, currentWave - 1);
                    }
                    enemyRemaining.Add(enemy);
                }
            }
        }
    }


}
