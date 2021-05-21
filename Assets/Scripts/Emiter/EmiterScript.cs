using UnityEngine;

public class EmiterScript : MonoBehaviour
{
    public GameObject bonus;  //тут хранится префаб бонуса

    public GameObject EnemyShip;  //тут харнится префаб вражеского корабля

    public GameObject[] asteroids;  //массив префабов астероидов
    
    public float minDelay, maxDelay;  //задержка между появлением(запуском) астероидов
    private float nextLaunchAsteroid; //время следующего запуска астероида

    public float minEnemyDelay, maxEnemyDalay;
    private float nextLaunchEnemy;

    public float minBonusDelay, maxBonusDelay;
    private float nextLaunchBonus;
    
    void Update()
    {
        if (Time.time > nextLaunchAsteroid)
        {
            //запуск астероида
            float positionZ = transform.position.z;
            float positionY = transform.position.y;
            float positionX = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);  //тут указуем что астроиды будут вылетать из разных мест по оси х эмитера

            var position = new Vector3(positionX, positionY, positionZ);  //составиляем вектор полёта астероида
            //Instantiate(asteroid, position, Quaternion.identity);  //создаём полёт астероида
            //выбирается какой астероид вылетит
            var chil = Random.Range(0, 3);   //задаю переменной случайное число в указаном диапазоне
            switch (chil)
            {
                case 0:
                    Instantiate(asteroids[0], position, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(asteroids[1], position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(asteroids[2], position, Quaternion.identity);
                    break;
            }
            nextLaunchAsteroid = Time.time + Random.Range(minDelay, maxDelay);  //переставляем время следующего запуска
        }

        if (Time.time > nextLaunchEnemy)
        {
            float positionZ = transform.position.z;
            float positionY = transform.position.y;
            float positionX = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);

            var enemyPosition = new Vector3(positionX, positionY, positionZ);
            Instantiate(EnemyShip, enemyPosition, Quaternion.identity);

            nextLaunchEnemy = Time.time + Random.Range(minEnemyDelay, maxEnemyDalay);  //переставляем время следующего запуска
        }

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
}