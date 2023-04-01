using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.AI;



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
    public TextMeshProUGUI enemiesLeftText;
    public TextMeshProUGUI currentWaveText;
    
    public LayerMask spawnLayerMask;
    
    public AudioSource soundManager;
    public AudioClip passWaveSound;





    private bool _firstTime = true;
    private Vector3 _firstPosition;

    void Start()
    {
        StartCoroutine(WaitBeforeTheNextRound());
        _firstPosition = spawnPoint.position;
        UpdateEnemiesLeftText();
        UpdateCurrentWaveText();
    }

    void Update()
    {
        if (enemyRemaining.All(x => x == null) && _firstTime == false)
        {
            StartCoroutine(WaitBeforeTheNextRound());
        }
        UpdateEnemiesLeftText();
    }

    Vector3 GetRandomPosition(float minDistance, float maxDistance, float minDistanceFromOtherEnemies, float colliderCheckRadius)
    {
        Vector3 randomPosition = spawnPoint.position;
        NavMeshHit hit;

        for (int i = 0; i < 100; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere;
            randomDirection.y = 0;
            randomPosition = spawnPoint.position + randomDirection.normalized * UnityEngine.Random.Range(minDistance, maxDistance);

            if (NavMesh.SamplePosition(randomPosition, out hit, colliderCheckRadius, NavMesh.AllAreas))
            {
                randomPosition = hit.position;

                bool isTooCloseToOtherEnemies = false;

                foreach (GameObject enemy in enemyRemaining)
                {
                    if (enemy != null && Vector3.Distance(randomPosition, enemy.transform.position) < minDistanceFromOtherEnemies)
                    {
                        isTooCloseToOtherEnemies = true;
                        break;
                    }
                }

                if (!isTooCloseToOtherEnemies && !Physics.CheckSphere(randomPosition, colliderCheckRadius, spawnLayerMask))
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
        if (soundManager != null && passWaveSound != null)
        {
            soundManager.PlayOneShot(passWaveSound);
        }
        enemyRemaining.Clear();
        currentWave++;
        UpdateCurrentWaveText();
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
                    UpdateEnemiesLeftText();
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
                    UpdateEnemiesLeftText();
                }
            }
        }
    }
    void UpdateEnemiesLeftText()
    {
        int enemiesLeft = enemyRemaining.Count(x => x != null);
        enemiesLeftText.text = $"Enemies : {enemiesLeft}";
    }
    
    void UpdateCurrentWaveText()
    {
        currentWaveText.text = $"Current Wave : {currentWave}";
    }



}
