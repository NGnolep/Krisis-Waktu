using TMPro;
using UnityEngine;
using UnityEngine.UI; // Needed for the Slider component
using System.Collections;

public class Tree : MonoBehaviour
{
    public int gold;
    public int health;
    public int maxHealth;
    public Slider treeHealthSlider; // Reference to the slider representing the tree's health
    public TextMeshProUGUI healthText; // Reference to the text displaying the tree's health
    public GameObject spawners; // Reference to the spawners object
    public EnemySpawn enemySpawnerScript; // Reference to the EnemySpawner script
    public ShopScript shopScript;

    [Header("Upgrade Related")]
    public GameObject shopPanel;
    public int lifeSteal = 0;
    public int shield = 0;
    public int addShield = 0;
    public float stunDuration;
    public bool enemiesAreStunned = false;
    public int stunCooldown;

    public int slowDuration;
    public bool enemiesAreSlowed = false;
    public float slowAmount = 0;
    public int slowCooldown;

    public int mitigationDuration;
    public int mitigationAmount;
    public bool isMitigatingDamage = false;
    public int mitigationCooldown;

    [Header("Skill Buttons")]
    public GameObject slow;
    public GameObject stun;
    public GameObject mitigate;
    
    public TextMeshProUGUI slowCooldownText;
    public TextMeshProUGUI stunCooldownText;
    public TextMeshProUGUI mitigationCooldownText;

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

        if(shopScript.stunUpgradedAmount > 0){
            stun.SetActive(true);
        }
        if(shopScript.slowUpgradedAmount > 0){
            slow.SetActive(true);
        }
        if(shopScript.mitigationUpgradedAmount > 0){
            mitigate.SetActive(true);
        }

        if(enemySpawnerScript.waveValue == 0){
            stun.SetActive(false);
            slow.SetActive(false);
            mitigate.SetActive(false);

            stunCooldown = 0;
            slowCooldown = 0;
            mitigationCooldown = 0;

            enemiesAreSlowed = false;
            enemiesAreStunned = false;
            isMitigatingDamage = false;

            shopPanel.SetActive(true);
        }
    }

    public void SlowSkill()
    {
        enemiesAreSlowed = true;
        StartCoroutine(Timer(slowDuration, 1));
    }
    public void MitigationSkill()
    {
        isMitigatingDamage = true;
        StartCoroutine(Timer(mitigationDuration, 2));
    }

    public void StunSkill()
    {
        enemiesAreStunned = true;
        StartCoroutine(Timer(stunDuration, 3));
    }

    public IEnumerator Cooldown(int purpose)
    {
        switch(purpose)
        {
            case 1:
                for(int i = slowCooldown; i > 0; i--)
                {
                    slowCooldownText.text = i.ToString();
                    yield return new WaitForSeconds(1);
                }
                break;
            case 2:
                for(int i = mitigationCooldown; i > 0; i--)
                {
                    mitigationCooldownText.text = i.ToString();
                    yield return new WaitForSeconds(1);
                }
                break;
            case 3:
                for(int i = stunCooldown; i > 0; i--)
                {
                    stunCooldownText.text = i.ToString();
                    yield return new WaitForSeconds(1);
                }
                break;
        }
    }

    public IEnumerator Timer(float duration, int purpose)
    {
        yield return new WaitForSeconds(duration);

        if(purpose == 1)
        {
            enemiesAreSlowed = false;
        }
        else if(purpose == 2)
        {
            isMitigatingDamage = false;
        }
        else if(purpose == 3)
        {
            enemiesAreStunned = false;
        } 
        Cooldown(purpose);
    }
}