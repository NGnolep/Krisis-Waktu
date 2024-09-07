using TMPro;
using UnityEngine;
using UnityEngine.UI; // Needed for the Slider component

public class Tree : MonoBehaviour
{
    public int gold;
    public int health;
    public int maxHealth;
    public Slider treeHealthSlider; // Reference to the slider representing the tree's health
    public TextMeshProUGUI healthText; // Reference to the text displaying the tree's health
    public GameObject spawners; // Reference to the spawners object

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        maxHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        treeHealthSlider.maxValue = maxHealth;
        treeHealthSlider.value = health;
        healthText.text = health.ToString() + " / " + maxHealth.ToString();
        
        if (health <= 0)
        {
            // Game over
            Debug.Log("Game Over");
            spawners.SetActive(false); // Disable the spawners
        }
    }
}
