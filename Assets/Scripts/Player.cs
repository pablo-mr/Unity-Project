using System;

using System.Collections; 
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float thrustForce = 5f;
    public float rotationSpeed = 120f;

    
    public static int SCORE = 0;
    public static int SPECIALATTACK = 0;
    public static float xBorderLimit, yBorderLimit;

    public GameObject bulletPrefab;
    public GameObject gun;

    Rigidbody _rigidbody;

    Vector2 thrustDirection;

    private bool isDisabled = false;

    void Start()
    {
        // rigidbody nos permite aplicar fuerzas en el jugador
        _rigidbody = GetComponent<Rigidbody>();

        yBorderLimit = Camera.main.orthographicSize+1;
        xBorderLimit = (Camera.main.orthographicSize+1) * Screen.width / Screen.height;
    }

    private void FixedUpdate()
    {
        if (isDisabled) return;
        // obtenemos las pulsaciones de teclado
        float rotation = Input.GetAxis("Rotate") * rotationSpeed * Time.deltaTime;
        float thrust = Input.GetAxis("Thrust") * thrustForce;
        // la dirección de empuje por defecto es .right (el eje X positivo)
        thrustDirection = transform.right;

        // rotamos con el eje "Rotate" negativo para que la dirección sea correcta
        transform.Rotate(Vector3.forward, -rotation);

        // añadimos la fuerza capturada arriba a la nave del jugador
        _rigidbody.AddForce(thrust * thrustDirection);
    }

    void Update()
    {
        if (isDisabled) return;

        var newPos = transform.position;
        if (newPos.x > xBorderLimit)
            newPos.x = -xBorderLimit+1;
        else if (newPos.x < -xBorderLimit)
            newPos.x = xBorderLimit-1;
        else if (newPos.y > yBorderLimit)
            newPos.y = -yBorderLimit+1;
        else if (newPos.y < -yBorderLimit)
            newPos.y = yBorderLimit-1;
        transform.position = newPos;

        if (Input.GetKeyDown(KeyCode.Q) && SPECIALATTACK>=5)
{

    
    SPECIALATTACK=0;
    // define el número de balas a disparar
    int numberOfBullets = 12;
    GameObject at = GameObject.FindGameObjectWithTag("SpecialAttack");
    at.GetComponent<Text>().text = "Second Weapon: " + (5-SPECIALATTACK);
    // calcula el ángulo entre cada bala (360 grados dividido por el número de balas)
    float angleStep = 360.0f / numberOfBullets;
    float currentAngle = 0f;

    for (int i = 0; i < numberOfBullets; i++)
    {
        // calcula la dirección de la bala basada en el ángulo actual
        float bulletDirXposition = transform.position.x + Mathf.Sin((currentAngle * Mathf.PI) / 180f);
        float bulletDirYposition = transform.position.y + Mathf.Cos((currentAngle * Mathf.PI) / 180f);

        Vector3 bulletVector = new Vector3(bulletDirXposition, bulletDirYposition, 0);
        Vector3 bulletMoveDirection = (bulletVector - transform.position).normalized; 

        // instancia la bala y configura su dirección
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().targetVector = bulletMoveDirection;

        // incrementa el ángulo actual para la siguiente bala
        currentAngle += angleStep;
    }
}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // al pulsar espacio, instanciamos una bala.
            GameObject bullet = Instantiate(bulletPrefab, gun.transform.position, Quaternion.identity);

            // cargamos el script Bullet del GO bullet en la variable balaScript            
            Bullet balaScript = bullet.GetComponent<Bullet>();

            // le damos dirección a la bala.
            balaScript.targetVector = transform.right;
        }
    }
    
    
    private void OnTriggerEnter(Collider collision)
    {   
        // si el jugador se choca con un asteroide/bala enemiga/nave enemiga significa que hemos muerto
        if (collision.gameObject.tag == "Enemy" ||collision.gameObject.tag=="EnemyBullet" || collision.gameObject.tag=="EnemyShip")
        {
            Application.LoadLevel(Application.loadedLevel);
            SCORE = 0; 
            SPECIALATTACK=0;
            EnemySpawner.bossAlive=false;
        }
        //se chocamos contra bala del segundo enemigo nos quedamos bloqueados 1.5 segundos
        if(collision.gameObject.tag=="EnemyBullet2"){
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity=Vector3.zero;
            StartCoroutine(DisablePlayer(1.5f));
        }
    }

    IEnumerator  DisablePlayer(float duration)
    {
        isDisabled = true; // Desactivamos el movimiento y disparo
        
        // Esperamos la cantidad de tiempo especificada
        yield return new WaitForSeconds(duration);
        
        isDisabled = false; // Reactivamos el movimiento y disparo
    }

}
