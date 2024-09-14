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
    public List<Enemy> spawnedEnemies = new List<Enemy>();     // List to keep track of spawned enemies
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

        if(currentCount == 0 && enemiesToSpawnLeft == 0)
        {
            treeScript.shopPanel.SetActive(true);
        }
    }

    private void InitializePoolOfWords(){
        poolOfWords = new List<string> {
        
        //easy
        "realm", "castle", "knight", "magic", "sword", "dragon", "forest", "shield", "quest", "crown", "tower", "spell", "stone", 
        "river", "light", "path", "king", "queen", "hero", "gate", "beast", "flame", "horse", "cloud", "star", "wood", "vine", "leaf", "sand", 
        "hill", "cave", "fire", "moon", "wind", "wave", "gold", "tree", "seed", "rope", "ship", "boat", "dock", "tent", "wall", 
        "map", "key", "coin", "pearl", "rock", "snow", "ice", "rain", "sun", "soil", "wolf", "bear", "hawk", "claw", "bow", "trap", "pick", "root", 
        "crop", "tool", "pond", "cart", "flag", "mine", "safe", "gem", "orb", "bell", "hut", "helm", "bridge", "ink", "horn", "mask", 
        "page", "nail", "glass", "chest", "hole", "pit", "den", "pool", "bed", "lamp", "leaf", "wing", "claw", "mark", "cave", "path", "gift", "lair", "herb", "band", "root", "time",
        //medium
        "wizard", "village", "kingdom", "potion", "crystal", "ancient", "warrior", "phoenix", "portal", "fortress", "guardian", "labyrinth", "horizon", "celestial", "enchant", "sanctuary", "oracle", "eclipse", "cathedral", "elemental", "artifact", "illusion", "citadel", 
        "dungeon", "sorcery", "mystic", "archer", "legacy", "goblin", "cavern", "canopy", "meadow", "crypt", "cavern", "relic", "ember", "knightly", "canyon", "mirror", "valley", "noble", "squire", "ruins", "meadow", "tempest", "ranger", "knightmare", "griffon", "druid", "seer", 
        "sentinel", "battalion", "falcon", "cavalier", "prophecy", "artifact", "chimera", "sapphire", "emerald", "basilisk", "scribe", "talisman", "relic", "marauder", "direwolf", "assassin", "shaman", "phantom", "talon", "ranger", "sacred", "herald", "mantle", "temple", "brigand", 
        "essence", "tyrant", "harbinger", "ethereal", "specter", "scimitar", "vizier", "alchemist", "harpy", "dragonfire", "glimmer", "coven", "minotaur", "shadowmancer", "scroll", "longbow", "scepter", "palisade", "alchemy", "blacksmith", "wanderer", "magical", "megalith", "apprentice", "demigod",
        //hard
        "dimension", "dominion", "necromancer", "prophecy", "apocalypse", "sovereign", "equilibrium", "incantation", "astralplane", "pantheon", "constellation", "resurrection", "metamorphosis", "obsidian", "leviathan", "transcendence", "primordial", "telekinesis", "annihilation", "chronomancer", "eldritch", 
        "subterranean", "millennium", "illumination", "incarnation", "fabrication", "oblivion", "consecration", "magnanimous", "imperious", "interminable", "arbitration", "fortification", "petrification", "transmogrify", "terraforming", "inquisitor", "emissary", "underworld", "divination", "cartographer", 
        "crimsonflame", "hallucination", "masquerade", "martyrdom", "invocation", "coronation", "incineration", "congregation", "amphitheater", "illumination", "extraplanar", "evocation", "harmonization", "respiration", "atonement", "crucifixion", "intercession", "purgatory", "gladiatorial", "resplendence",
        "magisterial", "purification", "exhumation", "telekinesis", "reinvention", "regeneration", "manifestation", "etherealplane", "pantheon", "dimensionless", "transmigration", "sovereignty", "clairvoyance", "extra-terrestrial", "enlightenment", "resurrection", "metamorphosis", "illumination", "consternation", 
        "rejuvenation", "purgatorial", "transfiguration", "petrification", "mythological", "rematerialize", "sanctification", "crystallization", "regeneration", "spectacular", "propitiation", "extraplanar", "apotheosis", "arcane", "eschatological", "underworld", "vanguard", "rejuvenation", "dimensional", "impermanence",
        //very hard
        "extradimensional", "hyperborean", "transmutation", "omnipotence", "cataclysmic", "antimatter", "impermanence", "psionicwave", "singularity", "telepathy", "quintessence", "maleficium", "bioluminescence", "crystallization", "eschatological", "pseudoscorpion", "serendipitous", "reconstitution", "antimaterialism", 
        "deconstructionism", "solipsistic", "metaphysical", "necromantic", "poltergeist", "chronomancy", "transdimensional", "celestialbeing", "interstellar", "telekinetic", "psychometric", "insurmountable", "invulnerability", "existentialism", "synchronicity", "transubstantiation", "eidolon", "hyperdimensional", "necromantic", 
        "pseudomythical", "crystallography", "meteorological", "astrophysical", "transcendental", "hyperspace", "interdimensional", "bioengineering", "extraorbital", "antimatterwave", "polyunsaturated", "pseudoscience", "misanthropic", "convolutional", "deconstructionist", "intergalactic", "transmutation", "metaphysical", 
        "quasistellar", "interplanetary", "levitation", "telepathic", "interdimensional", "cataclysmic", "omniscient", "pseudoarchaeology", "malfeasance", "antimaterial", "biotechnological", "premonition", "quantummechanical", "biomolecular", "esotericism", "extra-universal", "extradimensional", "hyperdimensional", "extraterrestrial", 
        "pseudoexistential", "omnipresent", "omniscient", "metaphysicality", "bioluminescent", "existential", "crystallographic", "quantization", "solipsisticwave", "etherealsphere", "quintessence", "pseudoscientific", "rejuvenation", "inconceivable", "serendipity", "introspective", "malfeasance", "transmutation", "deconstruction", 
        "psychometric", "telepathic", "clairvoyance", "metamorphosis", "extraordinary", "transcendence"
        
        };
    }

    private void SortPoolOfWords(){
        letterWords = new List<string>[9];  // Create an array of Lists for each word length (2 to 10 letters)

        // Initialize the lists in letterWords
        for (int i = 0; i < letterWords.Length; i++)
        {
            letterWords[i] = new List<string>();
        }

        foreach (var word in poolOfWords)
        {
            int wordLength = word.Length;
            // Word length between 2 and 10
            if (wordLength >= 2 && wordLength <= 10)
            {
                letterWords[wordLength - 2].Add(word);
                Debug.Log($"Added {word} to {wordLength}-letter words");
            }
            else if(wordLength < 2)
            {
                letterWords[0].Add(word);
            }
            else if(wordLength > 10)
            {
                letterWords[8].Add(word);
            }
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
        int randomInt;
        while (waveValue > 0){
            if(waveValue > 3){
                randomInt = Random.Range(1, 5);
                toSpawn.Add(randomInt);
            } else if(waveValue > 2){
                randomInt = Random.Range(1, 4);
                toSpawn.Add(randomInt);
            } else if(waveValue > 1){
                randomInt = Random.Range(1, 3);
                toSpawn.Add(randomInt);
            } else if(waveValue == 1){
                randomInt = 1;
                toSpawn.Add(randomInt);
            } else{
                break;
            }
            waveValue -= randomInt;
        }
    }
    
    // Initialize round settings, call after the round starts
    void Initialize()
    {
        currentCount = 0;
        treeScript.shield += treeScript.addShield;

        switch (currentRound)
        {
            //for example on what to put per round:
            case 1:
                waveValue = 20;
                randomizeEnemyCategory(waveValue);
                activeWords = letterWords[4];  // Example words
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
                    break;
                } else{
                    enemy.displayWord.text = enemy.toType2;
                    enemy.toType1 = enemy.toType2;
                    enemy.toType2 = null;
                    ClearTypedWord();
                }
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
        treeScript.health += treeScript.lifeSteal;
        spawnedEnemies.Remove(enemy);    // Remove from active enemy list
        treeScript.gold += enemy.goldDrop;
        Destroy(enemy.gameObject);       // Destroy enemy object
        currentCount--;
        ClearTypedWord();       // Clear the typed word 
    }
}
