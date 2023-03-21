using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 5.0f;

    public int enemyCount = 1;
    
    public GameObject enemy;

    private bool _waveIsDone = true;
    private void Update()
    {
        if (_waveIsDone)
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        _waveIsDone = false;

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy);
            

            yield return new WaitForSeconds(spawnRate);
        }

        enemyCount += 1;

        yield return new WaitForSeconds(timeBetweenWaves);

        _waveIsDone = true;
    }
}

