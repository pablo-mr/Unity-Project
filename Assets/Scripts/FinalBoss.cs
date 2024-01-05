
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab the boss will shoot
    private Transform player; // Reference to the player's transform
    public float moveSpeed = 2.0f; // Movement speed of the boss
    public float shootingInterval = 1.5f; // How often the boss shoots
    private float shootingTimer;

    public static int lifes=3;
    void Start()
    {
        // Initialize the shooting timer
        shootingTimer = shootingInterval;
    }

    void Update()
    {

        player=GameObject.FindGameObjectWithTag("Player").transform;
        // Move towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the rotation of the boss to always face the player
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        // Handle shooting at the player
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0)
        {
            ShootAtPlayer();
            shootingTimer = shootingInterval;
        }
    }

    void ShootAtPlayer()
    {
        // Instantiate the bullet and set it to move towards the player
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.targetVector = (player.position - transform.position).normalized;
    }

    
}
