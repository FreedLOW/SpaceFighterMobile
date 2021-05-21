using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceSript : MonoBehaviour
{
    Joystick joystick;  //переменная в которой хранится джойстик управления

    public GameObject shield;  //тут хранится объект щит

    private Rigidbody ship;  //объявление переменной корабля
    public float speed;  //объявление переменной скорости корабля, её можно менять в самом юнити
    public float tilt; //коэфициент поворота
    public float xMin, xMax, zMin, zMax;  //переменный регулировки перемещения корабля, т.е в каком диапазоне он может двигаться на карте

    public GameObject lazerShotPrefab;  //объект означающий чем будет стрелять, префаб лазера
    public GameObject lazerGunR;  //объект означающий откуда будет стрелять, правая пушка основная
    public GameObject lazerGunL;  //левая основная пушка
    public GameObject miniLazerShotPrefab;  //дополнительная пушка
    public GameObject miniGunR;
    public GameObject miniGunL;

    public float shotDelay;  //задержка между выстрелами
    private float nextShot; //переменная для определения времени между выстрелами
    
    public float damageLazerL, damageLazerR, auxiliaryLazerL, auxiliaryLazerR;

    private float moveHorizontal, moveVertical;

    void Start()
    {
        joystick = FindObjectOfType<VariableJoystick>();

        ship = GetComponent<Rigidbody>();  //доступ к кораблю (компоненту) и присваивание к переменной данные

        moveHorizontal = 0f;
        moveVertical = 0f;
    }

    void FixedUpdate()
    {
        MovementShip();
    }

    private void MovementShip()
    {
        //moveHorizontal = joystick.Horizontal * speed;  //создаётся переменная в которой определяется куда будет двигаться объект по горизонтале и с какой скоростью
        //moveVertical = joystick.Vertical * speed;  //тоже самое, но по вертикале

        if (joystick.Horizontal >= .5f)
            moveHorizontal = speed;
        else if (joystick.Horizontal <= -.5f)
            moveHorizontal = -speed;
        else if (joystick.Vertical >= .5f)
            moveVertical = speed;
        else if (joystick.Vertical <= -.5f)
            moveVertical = -speed;
        else
        {
            moveHorizontal = 0f;
            moveVertical = 0f;
        }

        ship.velocity = new Vector3(moveHorizontal, 0, moveVertical);  //задаю скорость и движение корабля

        //настройка наклона коробля при поворотах и движений вперёд и назад
        ship.rotation = Quaternion.Euler(ship.velocity.z * tilt, 0, -ship.velocity.x * tilt);  //реализация наклона по осям

        //ограничиваем зону полёта коробля, т.е. за какие пределы он не вылетит
        var positionX = Mathf.Clamp(ship.position.x, xMin, xMax);
        var positionZ = Mathf.Clamp(ship.position.z, zMin, zMax);
        var positionY = ship.position.y;  //ось у никак не надо ограничивать поэтому пишеться так

        //теперь ограниченую позицию нужно положить обратно к кораблю
        ship.position = new Vector3(positionX, positionY, positionZ);
    }

    public void MainGunShoot()  //реализация выстрела из основного орудия
    {
        if (Time.time > nextShot)  //условие следующее, если текущее время больше заданного(следующего выстрела), то можем стрелять
        {
            GameObject lazerR = Instantiate(lazerShotPrefab, lazerGunR.transform.position, Quaternion.identity);  //выстрел, используя этот метод сразу идёт тот объект которым хотим стрелять, далее задаётся позиция откуда будет производится выстрел, и его углы наклон в данном случае они не нужны
            GameObject lazerL = Instantiate(lazerShotPrefab, lazerGunL.transform.position, Quaternion.identity);
            nextShot = Time.time + shotDelay;  //переставляем время следующего выстрела

            lazerL.GetComponent<LazerScript>().Damage = damageLazerL / 2;
            lazerR.GetComponent<LazerScript>().Damage = damageLazerR / 2;
        }
    }

    public void AuxiliaryGunShoot()  //реализация выстрелов из маленьких пушек на крыльях
    {
        if (Time.time > nextShot)
        {
            GameObject lazerR = Instantiate(miniLazerShotPrefab, miniGunR.transform.position, Quaternion.identity);
            GameObject lazerL = Instantiate(miniLazerShotPrefab, miniGunL.transform.position, Quaternion.identity);
            nextShot = Time.time + shotDelay;

            lazerL.GetComponent<LazerScript>().Damage = auxiliaryLazerL / 2;
            lazerR.GetComponent<LazerScript>().Damage = auxiliaryLazerR / 2;
        }
    }

    //проверка на бонусы и появляется щит
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Bonus")
            shield.SetActive(true);
    }
}