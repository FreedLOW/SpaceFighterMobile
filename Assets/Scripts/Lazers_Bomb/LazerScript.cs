using UnityEngine;

public class LazerScript : MonoBehaviour
{
    public GameObject enemyExplosionEffect;
    public GameObject asteroidExplosion;

    public float speed;

    private float damage;
    public float Damage { get => damage; set => damage = value; }
    
    void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDestructable target = other.gameObject.GetComponent<IDestructable>();

        if (other.tag == "GameBoundary" || other.tag == "Player")
        {
            return;
        }
        else if (other.tag == "Asteroid")
        {
            Instantiate(asteroidExplosion, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "EnemyPlayer")
        {
            Instantiate(enemyExplosionEffect, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (target != null && other.tag == "Boss")
        {
            target.Hit(Damage);
            Destroy(gameObject);
        }
    }
}