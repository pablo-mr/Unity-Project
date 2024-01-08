using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public int speed = 10;
    public float maxLifeTime = 3;
    public Vector3 targetVector;

    void Start()
    {
        // nada más nacer, le damos unos segundos de vida, lo suficiente para salir de la pantalla
        Destroy(gameObject, maxLifeTime);
    }

    void Update()
    {
        // la bala se mueve en la dirección que tenía el jugador al disparar
        transform.Translate(targetVector * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {      
        //caso de impacto contra meteorito    
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            IncreaseScore();
        }

        //caso de impacto contra nave enemiga
        else if(other.gameObject.tag== "EnemyShip" && gameObject.tag!="EnemyBullet"){
            FinalBoss.lifes--;
            Destroy(gameObject);
            GameObject go = GameObject.FindGameObjectWithTag("EnemyHealth");
            go.GetComponent<Text>().text = "Boss Health: " + FinalBoss.lifes;
            if(FinalBoss.lifes==0){
                Destroy(other.gameObject);
                EnemySpawner.bossAlive=false;
                IncreaseScore();
                go.GetComponent<Text>().text = "";
            }
            
        }

    }
    
    public void IncreaseScore()
    {
        // cuando un asteroide es destruído, llama a esta función para darnos puntos.
        Player.SCORE++;
        if(Player.SPECIALATTACK<5)
            Player.SPECIALATTACK++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        // llamamos a esta función cada vez que ganamos puntos para actualizar el marcador
        GameObject go = GameObject.FindGameObjectWithTag("UI");
        go.GetComponent<Text>().text = "Score: " + Player.SCORE;

        // actualizamos el contador de puntos que faltan para desbloquear el ataque secundario
        GameObject at = GameObject.FindGameObjectWithTag("SpecialAttack");
        if(Player.SPECIALATTACK!=5)
            at.GetComponent<Text>().text = "Second Weapon: " + (5-Player.SPECIALATTACK);
        else
            at.GetComponent<Text>().text = "Second Weapon: Loaded" ;
    }

}
