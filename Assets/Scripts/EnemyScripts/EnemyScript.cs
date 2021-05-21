using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Rigidbody shipEnemy;
    public float minSpeed, maxSpeed;  //объявление переменной скорости корабля, её можно менять в самом юнити

    public GameObject EnemyExplosions;  //переменная для подключения взрыва вражеского коробля

    public GameObject lazerShot;  //объект означающий чем будет стрелять, префаб лазера
    public GameObject lazerEnemyR;  //объект означающий откуда будет стрелять, правая пушка основная
    public GameObject lazerEnemyL;  //левая основная пушка

    public float shootDelay;  //задержка между выстрелами
    private float nextShot; //переменная для определения времени между выстрелами

    void Start()
    {
        shipEnemy = GetComponent<Rigidbody>();

        shipEnemy.velocity = Vector3.back * Random.Range(minSpeed, maxSpeed);  //скорость движения вражеского корабля

        transform.rotation = Quaternion.Euler(0, 180, 0);  //задаю чтобы вражеский корабыль вылетал повёрнутым вниз и летел на корабль игрока
    }

    private void Update()
    {
        ShootEnemy();
    }

    private void ShootEnemy()
    {
        if (Time.time > nextShot)
        {
            Instantiate(lazerShot, lazerEnemyL.transform.position, Quaternion.identity);
            Instantiate(lazerShot, lazerEnemyR.transform.position, Quaternion.identity);
            nextShot = Time.time + shootDelay;  //переставляем время следующего выстрела
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameBoundary" || other.tag == "EnemyPlayer"  || other.tag == "Asteroid" || other.tag == "Bonus")
        {
            return;
        }

        if (other.tag == "Shield")  //если на игроке щит
        {
            GameObject.FindGameObjectWithTag("Shield").SetActive(false);  //дезактивирую щит
            Destroy(gameObject);  //уничтожаю вражеский корабль
            Instantiate(EnemyExplosions, transform.position, Quaternion.identity);  //создаём взрыв вражеского корабля
        }

        //взрыв корабля игрока
        if (other.tag == "Player")
        {
            Instantiate(EnemyExplosions, other.transform.position, transform.rotation);
            Destroy(other.gameObject);  //чничтожаем то с чем столкнулся вражеский корабль (лазерный выстрел или сам игрок)
            Destroy(gameObject);  //уничтожаем вражеский корабль
            HUD.Instance.ShowWindow(HUD.Instance.levelLoseWindow);
        }
        else
        {
            HUD.Instance.UpdateScore(10);  //добавляю очки
        }
    }
}