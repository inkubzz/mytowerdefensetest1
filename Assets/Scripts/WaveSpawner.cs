﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public int numberofWaves = 10;
    public float timeBetweenWaves = 8f;
    public Transform spawnPoint;
    public Transform enemyPrefab1;
    public Transform enemyPrefab2;
    public Transform enemyPrefab3;
    public Text waveNumberText;
    public Text waveCountdownText;

    private float countdown = 2f;
    private int waveIndex = 0;
    private bool levelEnded = false;

    private void Update()
    {
        if (levelEnded) return;

        if (waveIndex >= numberofWaves)
        {
            levelEnded = true;
            //countdown = timeBetweenWaves;
            waveCountdownText.text = "No more waves.";
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        if (waveIndex < numberofWaves)
        {
            countdown -= Time.deltaTime;
            waveCountdownText.text = Mathf.Floor(countdown + 1).ToString();
        }
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;

        // update UI element showing the current level number
        waveNumberText.text = "Wave: " + waveIndex + "/" + numberofWaves;

        // generate N enemies, where N is the number of the current wave
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.6f);
        }
    }

    private void SpawnRandomEnemy()
    {
        Transform enemyPrefab = null;

        System.Random random = new System.Random();
        int randomInt = random.Next(1, 4); // generates a number between 1 and 3
        switch (randomInt)
        {
            case 1:
            default:
                enemyPrefab = enemyPrefab1;
                break;
            case 2:
                enemyPrefab = enemyPrefab2;
                break;
            case 3:
                enemyPrefab = enemyPrefab3;
                break;
        }

        // in case only enemyPrefab1 is set and the others are not
        if (enemyPrefab == null) enemyPrefab = enemyPrefab1;
        
        if (enemyPrefab != null)
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab1, spawnPoint.position, spawnPoint.rotation);
    }
}
