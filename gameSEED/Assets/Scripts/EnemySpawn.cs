using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int currentRound;
    [SerializeField] Transform[] spawnpoints;
    public int enemiesToSpawnLeft; // total number of enemies to spawn this round
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<int> toSpawn;         // list of enemies to spawn (put in the ints for the categories)
    public List<string> words;                  // list of all possible words
    public int currentCount;                    // how many enemies are on the screen right now
    private List<Enemy> spawnedEnemies = new List<Enemy>();  // List to keep track of spawned enemies

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesToSpawnLeft > 0 && currentCount < 4)
        {
            SpawnEnemy();
        }

        // Check for player input and match with enemy words
        if (Input.anyKeyDown)
        {
            string typedWord = Input.inputString.ToLower();  // Get the player's typed input
            CheckWordMatch(typedWord);
        }
    }

    // Initialize round settings
    void Initialize()
    {
        currentCount = 0;
        currentRound = 1;  // For testing
        switch (currentRound)
        {
            case 1:
                toSpawn = new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                enemiesToSpawnLeft = 10;
                words = new List<string> { "example", "test", "word" };  // Example words
                break;
        }
    }

    // Spawn enemy at random spawnpoint and assign a word
    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab is not assigned.");
            return;
        }

        if (spawnpoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned.");
            return;
        }

        int randomIndex = Random.Range(0, spawnpoints.Length);
        GameObject enemyObject = Instantiate(enemyPrefab, spawnpoints[randomIndex].position, Quaternion.identity);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();

        if (enemyScript == null)
        {
            Debug.LogError("Enemy script is missing on the prefab.");
            Destroy(enemyObject);
            return;
        }

        // Set up enemy
        enemyScript.enemySpawnScript = this;
        string randomWord = GetRandomWord();  // Assign a random word from the list
        enemyScript.toType1 = randomWord;     // Set word in the script

        spawnedEnemies.Add(enemyScript);     // Add to the list of active enemies
        currentCount++;
        enemiesToSpawnLeft--;
        toSpawn.RemoveAt(0);
    }

    // Function to get a random word from the list
    string GetRandomWord()
    {
        if (words.Count == 0)
        {
            Debug.LogError("Words list is empty.");
            return string.Empty;
        }
        int randomWordIndex = Random.Range(0, words.Count);
        return words[randomWordIndex];
    }

    // Check if the typed word matches any enemy's word
    void CheckWordMatch(string typedWord)
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy.toType1.ToLower() == typedWord)  // Case-insensitive comparison
            {
                DestroyEnemy(enemy);
                break;  // Exit loop after killing one enemy
            }
        }
    }

    // Function to destroy enemy
    void DestroyEnemy(Enemy enemy)
    {
        spawnedEnemies.Remove(enemy);    // Remove from active enemy list
        Destroy(enemy.gameObject);       // Destroy enemy object
        currentCount--;
    }
}
