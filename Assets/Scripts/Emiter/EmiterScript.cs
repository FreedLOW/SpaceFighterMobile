using UnityEngine;

public class EmiterScript : MonoBehaviour
{
    public GameObject bonus;  //тут хранится префаб бонуса

    public GameObject[] enemyShips;  //тут харнится префаб вражеского корабля

    public GameObject[] asteroids;  //массив префабов астероидов
    
    public float minAsteroidDelay, maxAsteroidDelay;  //задержка между появлением(запуском) астероидов
    private float nextLaunchAsteroid; //время следующего запуска астероида

    public float minEnemyDelay, maxEnemyDalay;
    private float nextLaunchEnemy;

    public float minBonusDelay, maxBonusDelay;
    private float nextLaunchBonus;

    void Update()
    {
        LaunchDangerThing();

        LaunchEnemyShip();

        LaunchBonus();
    }

    private void LaunchBonus()
    {
        if (Time.time > nextLaunchBonus)
        {
            float positionZ = transform.position.z;
            float positionY = transform.position.y;
            float positionX = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);

            var bonusPosition = new Vector3(positionX, positionY, positionZ);

            Instantiate(bonus, bonusPosition, Quaternion.identity);

            nextLaunchBonus = Time.time + Random.Range(minBonusDelay, maxBonusDelay);
        }
    }

    private void LaunchEnemyShip()
    {
        if (Time.time > nextLaunchEnemy)
        {
            float positionZ = transform.position.z;
            float positionY = transform.position.y;
            float positionX = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);

            var enemyPosition = new Vector3(positionX, positionY, positionZ);

            var enemyRange = Random.Range(0, enemyShips.Length);
            Instantiate(enemyShips[enemyRange], enemyPosition, Quaternion.identity);  //создаю случайного врага

            nextLaunchEnemy = Time.time + Random.Range(minEnemyDelay, maxEnemyDalay);  //переставляем время следующего запуска
        }
    }

    private void LaunchDangerThing()
    {
        if (Time.time > nextLaunchAsteroid)
        {
            //запуск астероида
            float positionZ = transform.position.z;
            float positionY = transform.position.y;
            float positionX = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);  //тут указуем что астроиды будут вылетать из разных мест по оси х эмитера

            var position = new Vector3(positionX, positionY, positionZ);  //составиляем вектор полёта астероида

            //выбирается какой астероид вылетит
            var randomDangerThing = Random.Range(0, asteroids.Length);   //задаю переменной случайное число в указаном диапазоне
            Instantiate(asteroids[randomDangerThing], position, Quaternion.identity);  //создаю случайный астероид или бомбу

            nextLaunchAsteroid = Time.time + Random.Range(minAsteroidDelay, maxAsteroidDelay);  //переставляем время следующего запуска
        }
    }
}