using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class EnemySpawn : MonoBehaviour
{
    public int waveValue;
    public Tree treeScript;                     // Reference to the tree script
    public string typedWord;                    // what the player is typing
    public TextMeshProUGUI typedWordDisplay;    // display the typed word'
    public int enemiesToSpawnLeft;              // total number of enemies to spawn this round
    public int currentRound;                    // current round number
    public int timerTillNextSpawn;   // timer for spawning enemies
    [SerializeField] Transform[] spawnpoints;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<int> toSpawn;         // list of enemies to spawn (put in the ints for the categories)
    public int currentCount;    // how many enemies are on the screen right now
    private List<Enemy> spawnedEnemies = new List<Enemy>();     // List to keep track of spawned enemies
    public List<string> poolOfWords;  // list of all possible words
    public List<string>[] letterWords;  // list of all possible words
    public List<string> activeWords;
    // public int[] laneCounter = {0, 0, 0, 0};  // Counter for each lane

    // Start is called before the first frame update
    void Start()
    {
        InitializePoolOfWords();
        SortPoolOfWords();
        currentRound = 1;
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab is not assigned.");
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
        if (enemiesToSpawnLeft > 0 && currentCount < 4 || timerTillNextSpawn >= 5 && enemiesToSpawnLeft > 0)
        {
            SpawnEnemy();
            timerTillNextSpawn = 0;
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

    private void InitializePoolOfWords(){
        poolOfWords = new List<string> {"realm", "castle", "knight", "magic", "sword", "dragon", "forest", "shield", "quest", "crown", "tower", "spell", "stone", "river", "light", "path", "king", "queen", "hero", "gate", "beast", "flame", "horse", "cloud", "star", "wood", "vine", "leaf", "sand", "hill", "cave", "fire", "moon", "wind", "wave", "gold", "tree", "seed", "rope", "ship", "boat", "dock", "tent", "wall", "map", "key", "coin", "pearl", "rock", "snow", "ice", "rain", "sun", "soil", "wolf", "bear", "hawk", "claw", "bow", "trap", "pick", "root", "crop", "tool", "pond", "cart", "flag", "mine", "safe", "gem", "orb", "bell", "hut", "helm", "bridge", "ink", "horn", "mask", "page", "nail", "glass", "chest", "hole", "pit", "den", "pool", "bed", "lamp", "leaf", "wing", "claw", "mark", "cave", "path", "gift", "lair", "herb", "band", "root", "time", "wizard", "village", "kingdom", "potion", "crystal", "ancient", "warrior", "phoenix", "portal", "fortress", "guardian", "labyrinth", "horizon", "celestial", "enchant", "sanctuary", "oracle", "eclipse", "cathedral", "elemental", "artifact", "illusion", "citadel", "dungeon", "sorcery", "mystic", "archer", "legacy", "goblin", "cavern", "canopy", "crypt", "cavern", "relic", "ember", "knightly", "canyon", "mirror", "valley", "noble", "squire", "ruins", "meadow", "tempest", "ranger", "knightmare", "griffon", "druid", "seer", "sentinel", "battalion", "falcon", "cavalier", "sanctuary", "prophecy", "artifact", "chimera", "sapphire", "emerald", "basilisk", "elemental", "scribe", "talisman", "relic", "marauder", "direwolf", "assassin", "artifact", "shaman", "fortress", "illusion", "phantom", "talon", "ranger", "sacred", "herald", "mantle", "temple", "sentinel", "brigand", "essence", "tyrant", "harbinger", "ethereal", "specter", "scimitar", "vizier", "alchemist", "harpy", "dragonfire", "glimmer", "mantle", "brigand", "coven", "minotaur", "shadowmancer", "scroll", "longbow", "scepter", "dimension", "dominion", "necromancer", "prophecy", "apocalypse", "sovereign", "equilibrium", "incantation", "astralplane", "pantheon", "constellation", "resurrection", "metamorphosis", "obsidian", "leviathan", "transcendence", "primordial", "telekinesis", "annihilation", "chronomancer", "eldritch", "subterranean", "millennium", "illumination", "incarnation", "fabrication", "oblivion", "consecration", "magnanimous", "imperious", "interminable", "arbitration", "fortification", "petrification", "transmogrify", "terraforming", "inquisitor", "emissary", "underworld", "divination", "cartographer", "enchantment", "crimsonflame", "hallucination", "masquerade", "martyrdom", "invocation", "coronation", "incineration", "congregation", "amphitheater", "illumination", "extraplanar", "evocation", "atonement", "crucifixion", "intercession", "purgatory", "gladiatorial", "resplendence", "magisterial", "purification", "hallucination", "exhumation", "telekinesis", "reinvention", "regeneration", "manifestation", "etherealplane", "annihilation", "pantheon", "dimensionless", "transmigration", "incantation", "evocation", "subterranean", "sovereignty", "introspection", "quantization", "transcendence", "illumination", "resplendence", "consternation", "extradimensional", "incineration", "harmonization", "consecration", "sanctification", "petrification", "resuscitation", "enlightenment", "equilibrium", "clairvoyance", "extra-terrestrial", "resurrection", "metamorphosis", "constellation", "extradimensional", "excommunication", "hyperborean", "transmutation", "omnipotence", "etherealrealm", "cataclysmic", "antimatter", "impermanence", "psionicwave", "singularity", "telepathy", "quintessence", "maleficium", "bioluminescence", "crystallization", "eschatological", "pseudoscorpion", "serendipitous", "introspection", "reconstitution", "antimaterialism", "deconstructionism", "solipsistic", "metaphysical", "omniscient", "bioluminescent", "necromantic", "poltergeist", "chronomancy", "transdimensional", "celestialbeing", "interstellar", "telekinetic", "psychometric", "insurmountable", "invulnerability", "existentialism", "synchronicity", "transubstantiation", "eidolon", "hyperdimensional", "necromantic", "pseudomythical", "crystallography", "meteorological", "astrophysical", "transcendental", "hyperspace", "interdimensional", "bioengineering", "extraorbital", "antimatterwave", "polyunsaturated", "introspection", "pseudoscience", "misanthropic", "convolutional", "deconstructionist", "intergalactic", "transmutation", "metaphysical", "quasistellar", "interplanetary", "levitation", "telepathic", "interdimensional", "cataclysmic", "omnipresent", "teleological", "pseudoscientific", "existential", "chronostatic", "hyperspace", "dimensionality", "omniscience", "pseudoarchaeology", "extra-universal", "malfeasance", "antimaterial", "biotechnological", "premonition", "extradimensional", "hyperdimensional", "extraterrestrial", "quantummechanical", "biomolecular", "esotericism", "crystallography", "necromantic", "etherealsphere", "quintessence", "metaphysicality", "pseudoexistential", "solipsisticwave", "omnipotential", "cataclysmicforce", "existentialbeing", "pseudoscientist", "insurmountability"};
    }

    private void SortPoolOfWords(){
        letterWords = new List<string>[9];
        while(poolOfWords.Count > 0){
            // if(poolOfWords[0].Length == 2){
            //     if(letterWords[0] == null){
            //         Debug.Log("2 letter words is null");
            //         letterWords[0] = new List<string>();
            //     }
            //     letterWords[0].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 2 letter words");
            // } else if(poolOfWords[0].Length == 3){
            //     if(letterWords[1] == null){
            //         Debug.Log("3 letter words is null");
            //         letterWords[1] = new List<string>();
            //     }
            //     letterWords[1].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 3 letter words");
            // } else if(poolOfWords[0].Length == 4){
            //     if(letterWords[2] == null){
            //         Debug.Log("4 letter words is null");
            //         letterWords[2] = new List<string>();
            //     }
            //     letterWords[2].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 4 letter words");
            // } else if(poolOfWords[0].Length == 5){
            //     if(letterWords[3] == null){
            //         Debug.Log("5 letter words is null");
            //         letterWords[3] = new List<string>();
            //     }
            //     letterWords[3].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 5 letter words");
            // } else if(poolOfWords[0].Length == 6){
            //     if(letterWords[4] == null){
            //         Debug.Log("6 letter words is null");
            //         letterWords[4] = new List<string>();
            //     }
            //     letterWords[4].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 6 letter words");
            // } else if(poolOfWords[0].Length == 7){
            //     if(letterWords[5] == null){
            //         Debug.Log("7 letter words is null");
            //         letterWords[5] = new List<string>();
            //     }
            //     letterWords[5].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 7 letter words");
            // } else if(poolOfWords[0].Length == 8){
            //     if(letterWords[6] == null){
            //         Debug.Log("8 letter words is null");
            //         letterWords[6] = new List<string>();
            //     }
            //     letterWords[6].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 8 letter words");
            // } else if(poolOfWords[0].Length == 9){
            //     if(letterWords[7] == null){
            //         Debug.Log("9 letter words is null");
            //         letterWords[7] = new List<string>();
            //     }
            //     letterWords[7].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 9 letter words");
            // } else{
            //     if(letterWords[8] == null){
            //         Debug.Log("10 letter words is null");
            //         letterWords[8] = new List<string>();
            //     }
            //     letterWords[8].Add(poolOfWords[0]);
            //     Debug.Log("Added " + poolOfWords[0] + " to 10 letter words");
            // } 
            poolOfWords.RemoveAt(0);
        }
    }

    // Increment timer
    private IEnumerator IncrementTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timerTillNextSpawn++;
        }
    }

    void randomizeEnemyCategory(int waveValue){
        while (waveValue > 0){
            if(waveValue > 4){
                toSpawn.Add(Random.Range(1, 5));
            } else if(waveValue > 3){
                toSpawn.Add(Random.Range(1, 4));
            } else if(waveValue > 2){
                toSpawn.Add(Random.Range(1, 3));
            } else if(waveValue > 1){
                toSpawn.Add(1);
            }
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
                waveValue = 20;
                randomizeEnemyCategory(waveValue);
                activeWords = letterWords[0];  // Example words
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
        // Get a random spawnpoint
        Vector3 spawnPosition;
        bool isTooClose;
        do{
            isTooClose = false;
            spawnPosition = new Vector3(this.transform.position.x, Random.Range(-4, 4) + (Random.Range(0,10) / 10), 0);
            foreach (var enemy in spawnedEnemies)
            {
                if (Vector3.Distance(spawnPosition, enemy.transform.position) < 0.8)
                {
                    isTooClose = true;
                    break;
                }
            }
        } while(isTooClose);
        
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.enemySpawnScript = this;

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
    public string GetRandomWord()
    {
        if (activeWords.Count == 0)
        {
            Debug.LogError("Words list is empty.");
            return string.Empty;
        }
        int randomWordIndex = Random.Range(0, activeWords.Count);
        return activeWords[randomWordIndex];
    }

    // Check if the typed word matches any enemy's word
    void CheckWordMatch(string typedWord)
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy.toType1.ToLower() == typedWord) // Case-insensitive comparison
            {
                if(enemy.toType2 == null){
                    DestroyEnemy(enemy);
                } else{
                    enemy.displayWord.text = enemy.toType2;
                    enemy.toType1 = enemy.toType2;
                    enemy.toType2 = null;
                    ClearTypedWord();
                }
                break;
            } 
        }
    }

    public void ClearTypedWord()
    {
        typedWord = string.Empty;
    }

    // Function to destroy enemy
    public void DestroyEnemy(Enemy enemy)
    {
        spawnedEnemies.Remove(enemy);    // Remove from active enemy list
        Destroy(enemy.gameObject);       // Destroy enemy object
        currentCount--;
        ClearTypedWord();       // Clear the typed word 
    }
}
