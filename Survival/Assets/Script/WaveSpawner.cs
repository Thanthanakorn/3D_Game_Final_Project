using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveSpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<GameObject> enemyRemaining;
    public Transform spawnPoint;

    public int currentWave;
    public float timeBetweenWaves = 5;

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

    void StartNextWave()
    {
        StartCoroutine(currentWave % 4 == 0 ? IncreaseEnemies() : LevelUpEnemies());
    }

    private IEnumerator WaitBeforeTheNextRound()
    {
        _firstTime = true;
        yield return new WaitForSecondsRealtime(timeBetweenWaves);
        enemyRemaining.Clear();
        currentWave++;
        transform.position = _firstPosition;
        StartNextWave();
    }

    private IEnumerator IncreaseEnemies()
    {
        for (int i = 0; i < (currentWave / 4) + 1; i++)
        {
            foreach (GameObject enemyPrefab in enemyPrefabs)
            {
                transform.position += Vector3.right * 20;
                if (_firstTime)
                {
                    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    enemyRemaining.Add(enemy);
                    _firstTime = false;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(3f);
                    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    enemyRemaining.Add(enemy);
                }
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator LevelUpEnemies()
    {
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            transform.position += Vector3.right * 20;
            if (_firstTime)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                if (currentWave > 1)
                {
                    enemy.GetComponent<EnemyStats>().healthLevel *= 1.5f;
                    enemy.GetComponent<EnemyStats>().attackLevel *= 1.5f;
                }
                enemyRemaining.Add(enemy);
                _firstTime = false;
            }
            else
            {
                yield return new WaitForSecondsRealtime(3f);
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                if (currentWave > 1)
                {
                    enemy.GetComponent<EnemyStats>().healthLevel *= 1.5f;
                    enemy.GetComponent<EnemyStats>().attackLevel *= 1.5f;
                }
                enemyRemaining.Add(enemy);
            }
        }
    }
}