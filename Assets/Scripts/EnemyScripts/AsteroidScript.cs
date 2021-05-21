using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public float rotationSpeed;  //переменная для корекции скорости астероида
    public float minSpeed, maxSpeed;  //скорость движения астероида
    public GameObject asteroidExsplosion;  //переменная для подключения эффекта взрыва астероида
    public GameObject shipExsplosions;  //переменная для подключения взрыва коробля

    //на астероиде в риджитбади где AngularDrag надо ставить 0, эта строчка означает замедление вращения, в данном моменте это не нужно
    void Start()
    {
        //реализуем вращение астероида
        //GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * rotationSpeed;  //каждый запуск игры астероид вращается в разные стороны с помощью функции рандом
        var asteroid = GetComponent<Rigidbody>();  
        asteroid.angularVelocity = Random.insideUnitSphere * rotationSpeed;  //другой способ реализации той строки что выше, функция таже

        //реализация движения астероида со случайной скоростью, её пределы заданы в юнити
        asteroid.velocity = Vector3.back * Random.Range(minSpeed, maxSpeed);
    }
    
    //срабатывает при столкновении
    private void OnTriggerEnter(Collider other)
    {
        //если тэг объекта(астероида) равняется "геймбаундри", то мы просто выходим, ничего не уничтожаем, и если тэг равен "астероид" то астроиды друг друга не уничтожают
        if(other.tag == "GameBoundary" || other.tag == "Asteroid"  || other.tag == "EnemyPlayer" || other.tag == "Bonus")
        {
            return;
        }
        else if (other.tag == "Shield")
        {
            GameObject.FindGameObjectWithTag("Shield").SetActive(false);  //дезактивирую щит
            Destroy(gameObject);  //уничтожаю астероид
            Instantiate(asteroidExsplosion, transform.position, Quaternion.identity);  //создаём взрыв астероида
        }
        else if (other.tag == "Player")
        {
            Destroy(other.gameObject);  //чничтожаем то с чем столкнулись (лазерный выстрел)
            Destroy(gameObject);  //уничтожаем астероид
            Instantiate(shipExsplosions, transform.position, Quaternion.identity);
            Instantiate(asteroidExsplosion, transform.position, Quaternion.identity);  //создаём взрыв астероида
            HUD.Instance.ShowWindow(HUD.Instance.levelLoseWindow);
        }
        else
        {
            HUD.Instance.UpdateScore(5);  //обновляю очки, т.е. псоле уничтожения астероида добавится 5 очков
        }
    }
}