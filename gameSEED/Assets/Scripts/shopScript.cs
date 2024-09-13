using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shopScript : MonoBehaviour
{
    [Header("Script References")]
    public EnemySpawn enemySpawn;

    [Header("Object References")]
    public TextMeshProUGUI Gold;
    public GameObject shopPanel;
    public GameObject HPButton;
    public GameObject shieldButton;
    public GameObject lifeSteal;
    public GameObject stun;
    public GameObject mitigation;
    public GameObject slow;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(enemySpawn.waveValue == 0)
        {
            shopPanel.SetActive(true);
            
        }
    }
}