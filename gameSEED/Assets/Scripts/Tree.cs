using TMPro;
using UnityEngine;
using UnityEngine.UI; // Needed for the Slider component

public class Tree : MonoBehaviour
{
    public int shield = 0;
    public int gold;
    public int health;
    public int maxHealth;
    public Slider treeHealthSlider; // Reference to the slider representing the tree's health
    public TextMeshProUGUI healthText; // Reference to the text displaying the tree's health
    public GameObject spawners; // Reference to the spawners object
    public EnemySpawn enemySpawnerScript; // Reference to the EnemySpawner script
    public GameObject shopPanel;
    public bool isMitigatingDamage = false;
    public bool enemiesAreStunned = false;
    public bool enemiesAreSlowed = false;
    public int movementSpeedMultiplier = 1;
    public int lifeSteal = 0;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawnerScript = spawners.GetComponent<EnemySpawn>();
        health = 100;
        maxHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(health > maxHealth){
            health = maxHealth;
        }
        treeHealthSlider.maxValue = maxHealth;
        treeHealthSlider.value = health;
        healthText.text = health.ToString() + " / " + maxHealth.ToString();
        if(enemySpawnerScript.waveValue == 0)
        {
            shopPanel.SetActive(true); 
        }

        if (health <= 0)
        {
            // Game over
            // Debug.Log("Game Over");
            spawners.SetActive(false); // Disable the spawners
        }
    }
}
