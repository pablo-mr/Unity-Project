
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public GameObject bulletPrefab; 
    private Transform player; 
    public float moveSpeed = 2.0f; 
    public float shootingInterval = 1.5f; 
    private float shootingTimer;

    public static int lifes;
    void Start()
    {
        shootingTimer = shootingInterval;
    }

    void Update()
    {

        player=GameObject.FindGameObjectWithTag("Player").transform;
        
        // Ajustar la direccion para moverse hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

       
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        //disparar al jugador si toca
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0)
        {
            ShootAtPlayer();
            shootingTimer = shootingInterval;
        }
    }

    void ShootAtPlayer()
    {
        //Instanciar la bala y dirigirla hacia el jugador
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.targetVector = (player.position - transform.position).normalized;
    }

    
}
