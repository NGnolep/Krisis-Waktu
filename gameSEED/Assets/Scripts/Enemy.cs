using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySpawn enemySpawnScript;
    public int category;
    public float speed;
    public bool canMove;
    public float damageToTree;
    public float goldDrop;
    public string toType1;  // Word associated with this enemy

    void Start()
    {
        canMove = true;

        // Set enemy attributes based on category
        switch (category)
        {
            case 1:
                speed = 10f;
                damageToTree = 10;
                goldDrop = 10;
                break;
            case 2:
                speed = 20f;
                damageToTree = 25;
                goldDrop = 30;
                break;
            case 3:
                speed = 20f;
                damageToTree = 50;
                goldDrop = 60;
                break;
            case 4:
                speed = 30f;
                damageToTree = 100;
                goldDrop = 100;
                break;
            case 5:  // Boss
                damageToTree = 999999999;
                break;
            default:
                break;
        }

        // Optionally, you can set up other initial properties here
    }

    void Update()
    {
        // Move left if canMove is true
        if (canMove)
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        canMove = false;
        if (other.tag == "Tree")
        {
            Debug.Log("Tree hit for " + damageToTree + " damage");
            Destroy(gameObject);  // Destroy the enemy after hitting the tree
        }
    }
}
