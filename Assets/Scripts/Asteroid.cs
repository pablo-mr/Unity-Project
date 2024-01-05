using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed = 3f;
    public int points = 5;
    public float maxLifeTime = 6;

    
    void Start()
    {
        // máximos segundos de vida para los asteroides
        Destroy(gameObject, maxLifeTime);
        GetComponent<Rigidbody>().AddForce(new Vector3(0,-speed*100,0));

    }
    
}
