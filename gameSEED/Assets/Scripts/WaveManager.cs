using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Wave
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public float SpawnRate;
}

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private float timeBetweenWaves = 5f;
    private float countdown = 2f;

    void Update()
    {
        if(!isSpawning)
        {
            if(countdown <= 0f)
            {
                
            }
        }
    }

}
