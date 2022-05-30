using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] wave;
    [SerializeField] private Transform[] spawnPoints;
    public GameObject player;


    public TMP_Text waveName;
    public TMP_Text countDown;
    public TMP_Text waveFinished;
    public TMP_Text enemiesLeft;
    public TMP_Text TimerText;
    public GameObject gameFinishedPanel;

    public static int enemyHealth;

    public GameObject enemiesLeftGameObject;

    private bool isTimerActive = true;
    private float currentTime;

    private Wave currentWave;
    private int currentWaveIndex;

    private bool canSpawn = true;
    private bool isSetup = false;

    private float nextSpawnTime;
    private GameObject[] totalEnemiesInWave;

    private void Start()
    {
        currentTime = 6f;
        KilledEnemies.currentEnemies = wave[0].numberOfEnemies;
        enemiesLeft.text = KilledEnemies.currentEnemies.ToString();
        enemyHealth = 30;
    }

    private void Update()
    {
        currentWave = wave[currentWaveIndex];   
        SpawnWave();
        totalEnemiesInWave = GameObject.FindGameObjectsWithTag("Enemy");

        if(totalEnemiesInWave.Length == 0 && !canSpawn )
        {
            if (currentWaveIndex >= (wave.Length-1))
            {
                GameFinished();
            }
            else
            {
                if (!isSetup)
                    SetupBeforeCountdown();
                StartCountdown();
            }
        }
        UpdateUI();
    }

    private void GameFinished()
    {
        Cursor.lockState = CursorLockMode.None;
        enemyHealth = 30;
        Enemy.damageToPlayer = 5;
        gameFinishedPanel.SetActive(true);
        Destroy(player.gameObject);
        Destroy(this.gameObject);
    }

    private void SetupBeforeCountdown()
    {
        isSetup = true;
        waveName.gameObject.SetActive(false);
        waveFinished.gameObject.SetActive(true);
        countDown.gameObject.SetActive(true);
    }


    private void UpdateUI()
    {
        enemiesLeft.text = KilledEnemies.currentEnemies.ToString();
    }

    private void SpawnNextWave()
    {
        countDown.gameObject.SetActive(false);
        waveName.gameObject.SetActive(true);
        waveFinished.gameObject.SetActive(false);
        currentWaveIndex++;
        KilledEnemies.currentEnemies = wave[currentWaveIndex].numberOfEnemies;
        waveName.text = wave[currentWaveIndex].waveName;
        canSpawn = true;
        isSetup = false;
        isTimerActive = true;
        currentTime = 6f;
        Enemy.damageToPlayer = wave[currentWaveIndex].damageToPlayer;
        SetEnemyHealth();
    }

    public void SetEnemyHealth()
    {
        enemyHealth = wave[currentWaveIndex].enemyHealth;
    }

    private void StartCountdown()
    {
        if (isTimerActive)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                isTimerActive = false;
                SpawnNextWave();
            }
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            TimerText.text = time.Seconds.ToString();
        }
    }

    private void SpawnWave()
    {
        if(canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.typeOfEnemies[UnityEngine.Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Length)];
            Instantiate(randomEnemy, randomSpawnPoint.position, Quaternion.identity);
            currentWave.numberOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;
            if(currentWave.numberOfEnemies==0)
            {
                canSpawn = false;
            }
        }
    }
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public int numberOfEnemies;
    public int damageToPlayer;
    public int enemyHealth;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public static class KilledEnemies
{
    public static int currentEnemies = 0;
}
