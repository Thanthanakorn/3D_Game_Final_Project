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
        currentWave++;
        StartCoroutine(StartNextWave());
        _firstPosition = spawnPoint.position;
    }

    void Update()
    {
        if (enemyRemaining.All(x => x == null) && _firstTime == false)
        {
            StartCoroutine(WaitBeforeTheNextRound());
        }
    }

    IEnumerator StartNextWave()
    {
        for (int i = 0; i < currentWave; i++)
        {
            foreach (GameObject enemyPrefab in enemyPrefabs)
            {
                transform.position += Vector3.forward * 10;
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

    IEnumerator WaitBeforeTheNextRound()
    {
        _firstTime = true;
        yield return new WaitForSecondsRealtime(timeBetweenWaves);
        enemyRemaining.Clear();
        currentWave++;
        transform.position = _firstPosition;
        StartCoroutine(StartNextWave());
    }
}