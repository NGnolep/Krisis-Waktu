using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class EnemySpawn : MonoBehaviour
{
    public Tree treeScript;                     // Reference to the tree script
    public string typedWord;                    // what the player is typing
    public TextMeshProUGUI typedWordDisplay;    // display the typed word
    public int enemiesToSpawnLeft;              // total number of enemies to spawn this round
    public int currentRound;                    // current round number
    public List<string> words;  // list of all possible words
    public int timer;   // timer for spawning enemies
    [SerializeField] Transform[] spawnpoints;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<int> toSpawn;         // list of enemies to spawn (put in the ints for the categories)
    public int currentCount;    // how many enemies are on the screen right now
    private List<Enemy> spawnedEnemies = new List<Enemy>();     // List to keep track of spawned enemies
    public int[] laneCounter = {0, 0, 0, 0};  // Counter for each lane

    // Start is called before the first frame update
    void Start()
    {
        currentRound = 1;
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
        //for testing because this is going to later be called only when you press a button to start a round, not at start
        Initialize();
        StartCoroutine(IncrementTimer());
    }

    // Update is called once per frame
    void Update()
    {
        typedWordDisplay.text = typedWord;  // Display the typed word
        //insert spawn condition here, like if current count < 4 or sth
        if (enemiesToSpawnLeft > 0 && currentCount < 4 || timer >= 5 && enemiesToSpawnLeft > 0)
        {
            SpawnEnemy();
            timer = 0;
        }

        // Check for player input and match with enemy words
        if(Input.GetKeyDown(KeyCode.Backspace)){
            if(typedWord.Length > 0)
            {
                typedWord = typedWord.Remove(typedWord.Length - 1);
            }
        }
        else if(Input.anyKeyDown)
        {
            typedWord += Input.inputString.ToLower();  // Get the player's typed input
            CheckWordMatch(typedWord);
        }
    }

    // Increment timer
    private IEnumerator IncrementTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer++;
        }
    }

    // Initialize round settings, call after the round starts
    void Initialize()
    {
        currentCount = 0;
        switch (currentRound)
        {
            //for example on what to put per round:
            case 1:
                toSpawn = new List<int> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
                words = new List<string> { "example", "test", "word" };  // Example words
                break;
            
            case 2:
                break;
            
            case 3:
                break;
            
            case 4:
                break;

            case 5:
                break;

            case 6:
                break;

            case 7:
                break;
            
            case 8:
                break;
            
            case 9:
                break;

            case 10:
                break;
            
            case 11:
                break; 
            
            case 12:
                break;

            default:
                break;
        }
        enemiesToSpawnLeft = toSpawn.Count;
        for(int i = 0; i < 4; i++){
            SpawnEnemy();
        }
    }

    // Spawn enemy at random spawnpoint and assign a word
    void SpawnEnemy()
    {
        int laneToSpawn = laneCounter
            .Select((count, index) => new { Count = count, Index = index })
            .OrderBy(x => x.Count)
            .First().Index;
        GameObject enemyObject = Instantiate(enemyPrefab, spawnpoints[laneToSpawn].position, Quaternion.identity);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.enemySpawnScript = this;
        laneCounter[laneToSpawn]++;
        enemyScript.lane = laneToSpawn; 

        // Set up enemy
        if (enemyScript == null)
        {
            Debug.LogError("Enemy script is missing on the prefab.");
            Destroy(enemyObject);
            return;
        }
        if(treeScript == null)
        {
            Debug.LogError("Tree script is missing.");
            return;
        }

        string randomWord = GetRandomWord();  // Assign a random word from the list
        enemyScript.toType1 = randomWord;     // Set word in the script

        spawnedEnemies.Add(enemyScript);     // Add to the list of active enemies
        currentCount++;
        enemiesToSpawnLeft--;
        enemyScript.category = toSpawn[0];
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
        typedWord = string.Empty;       // Clear the typed word
    }
}
