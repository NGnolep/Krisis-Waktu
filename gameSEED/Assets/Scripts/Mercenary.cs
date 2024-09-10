using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary : MonoBehaviour
{
    public float detectRange = 20f;   // Range to detect enemies
    public float attackRange = 2f;    // Range at which the mercenary attacks the enemy
    public float moveSpeed = 5f;      // Speed at which the mercenary moves
    private GameObject targetEnemy;
    private Camera mainCamera;

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = detectRange;
        targetEnemy = null;

        foreach (GameObject enemy in enemies)
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
    private bool IsEnemyOnScreen(GameObject enemy)
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
                Destroy(targetEnemy);
            }
            else
            {
                // Move towards the enemy
                Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }
}
