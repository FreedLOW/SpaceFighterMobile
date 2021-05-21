using UnityEngine;

public class BonusScript : MonoBehaviour
{
    private Rigidbody bonus;

    public float minSpeed, maxSpeed;

    private void Start()
    {
        bonus = gameObject.GetComponent<Rigidbody>();

        bonus.velocity = Vector3.back * Random.Range(minSpeed, maxSpeed);  //задаю скорость перемещение бонусу
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.tag == "Player")  //если тэг того с чем столкнулся бонус ранвяется "игрок", то:
        {
            Destroy(gameObject);  //уничтожаю объект бонус
        }
    }
}