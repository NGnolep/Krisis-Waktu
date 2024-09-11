using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mercenary : MonoBehaviour
{
    [SerializeField] EnemySpawn spawnerScript;
    public float detectRange = 20f;   // Range to detect enemies
    public float attackRange = 2f;    // Range at which the mercenary attacks the enemy
    public float moveSpeed = 5f;      // Speed at which the mercenary moves
    private Enemy targetEnemy;
    private Camera mainCamera;
    bool isWaiting = false;
    
    private void Start()
    {
        mainCamera = Camera.main;  // Get reference to the main camera
    }

    private void Update()
    {
        DetectEnemies();
        MoveToEnemy();
    }

    // Detect the nearest enemy with the "Enemy" tag that is on screen
    private void DetectEnemies()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        Enemy[] enemies = new Enemy[enemyObjects.Length];

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            enemies[i] = enemyObjects[i].GetComponent<Enemy>();
        }

        float closestDistance = detectRange;
        targetEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (IsEnemyOnScreen(enemy))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    targetEnemy = enemy;
                }
            }
        }
    }

    // Check if the enemy is within the camera's view
    private bool IsEnemyOnScreen(Enemy enemy)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);
        bool isOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return isOnScreen;
    }

    // Move towards the enemy if one is found
    private void MoveToEnemy()
    {
        if (targetEnemy != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetEnemy.transform.position);

            // If the enemy is within attack range, destroy the enemy
            if (distanceToTarget <= attackRange)
            {
                if(!isWaiting){
                    Coroutine waitCoroutine = StartCoroutine(Wait());
                }
            }
            else
            {
                // Move towards the enemy
                Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    private IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1f);
        spawnerScript.spawnedEnemies.Remove(targetEnemy);    // Remove from active enemy list
        Destroy(targetEnemy.gameObject);       // Destroy enemy object
        spawnerScript.treeScript.gold += targetEnemy.goldDrop / 2;           // Add gold to player's gold count
        spawnerScript.currentCount--;
        isWaiting = false;
    }
}
