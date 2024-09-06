using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // public Animator animator;
    // public AudioManager audioManager;
    public int currentRound;
    [SerializeField] Transform[] spawnpoints;
    public Sprite[] enemySprites;
    public int enemiesToSpawnLeft; //total number of enemies to spawn this round (starts from full, goes to 0)
    [SerializeField] GameObject enemyPrefab;  
    [SerializeField] List<int> toSpawn;         //list of enemies to spawn (put in the ints for the categories)
    public List<string> words;                  //list of all possible words
    public int currentCount;                    //how many enemies are on the screen right now

    // Start is called before the first frame update
    void Start()
    {
        //for testing
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //insert spawn condition here, like if current count < 4 or sth
        if(enemiesToSpawnLeft > 0 && currentCount < 4){
            SpawnEnemy();
        }
    }

    // Call after a round starts
    void Initialize()
    {
        currentCount = 0;
        currentRound = 1; //for testing
        switch(currentRound){
            //for example:
            case 1: 
                toSpawn = new List<int> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
                enemiesToSpawnLeft = 10;
                break;
            
        }

    }

    void SpawnEnemy(){
         int randomIndex = Random.Range(0, 4);
        GameObject enemy = Instantiate(enemyPrefab, spawnpoints[randomIndex].position, Quaternion.identity); //spawning in lane 1-4
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.enemySpawnScript = this;
        currentCount++;
        enemiesToSpawnLeft--;
        enemyScript.category = toSpawn[0];
        toSpawn.RemoveAt(0);
    }
}
