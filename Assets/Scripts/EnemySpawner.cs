using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject bossPrefab1;
    public GameObject bossPrefab2;
    public float spawnRatePerMinute = 30;
    public float spawnRateIncrement = 1;

    private float spawnNext = 0;

    public static bool bossAlive= false;

    void Update() 
    {

        if(Player.SCORE!=0  && Player.SCORE%5==0 && !bossAlive){
            bossAlive = true; // marcamos que el jefe está vivo para evitar duplicados
            FinalBoss.lifes=3;
            GameObject bossPrefab=bossPrefab1;
        if(Player.SCORE%10==0)
            bossPrefab=bossPrefab2;
        
        // calculamos la posición del jefe enfrente del jugador
        Vector3 bossPosition = new Vector3(0, Player.yBorderLimit / 2, 0); 
        
        // instanciamos al jefe en esta nueva posición y con la rotación deseada
        Instantiate(bossPrefab, bossPosition, Quaternion.Euler(0, 0, 180));
        // añadimos un texto en pantalla con la vida del jefe
        GameObject go = GameObject.FindGameObjectWithTag("EnemyHealth");
        go.GetComponent<Text>().text = "Boss Health: " + FinalBoss.lifes;
        
        }
        // instanciamos enemigos sólo si ha pasado tiempo suficiente desde el último.
        if (!bossAlive && Time.time > spawnNext)
        {
            // indicamos cuándo podremos volver a instanciar otro enemigo
            spawnNext = Time.time + 60 / spawnRatePerMinute;
            // con cada spawn incrementamos los asteroides por minuto para incrementar la dificultad
            spawnRatePerMinute += spawnRateIncrement;

            // guardamos un punto aleatorio entre las esquinas superiores de la pantalla
            var rand = Random.Range(-Player.xBorderLimit, Player.xBorderLimit);
            var spawnPosition = new Vector2(rand, Player.yBorderLimit);
            
            // instanciamos el asteroide en el punto y con el ángulo aleatorios
            Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        }
        
    }
}
