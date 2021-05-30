using UnityEngine;

public class PlayerSpaceSript : MonoBehaviour
{
    public GameObject shield;  //тут хранится объект щит

    private Rigidbody ship;  //объявление переменной корабля

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
    
    public float damageLazer, damageAuxiliaryLazer;

    private float positionX, positionY, positionZ;

    //управление с помощью джойстика:
    [SerializeField] Joystick joystick = null;  //переменная в которой хранится джойстик управления
    private float moveHorizontal, moveVertical;
    public float speedJoystick;  //объявление переменной скорости корабля, её можно менять в самом юнити
    public static bool joystickControl = false;
    public static bool JoystickControl { get => joystickControl; set => joystickControl = value; }

    //управление с помощью акселирометра:
    public float speedAcceleration;  //скорость движения коробля при управлении через акселерометр
    Quaternion callibrateRotation;
    public static bool accelerationControl = false;
    public static bool AccelerationControl { get => accelerationControl; set => accelerationControl = value; }

    void Start()
    {
        ship = GetComponent<Rigidbody>();  //доступ к кораблю (компоненту) и присваивание к переменной данные

        moveHorizontal = 0f;
        moveVertical = 0f;

        CalibrateAcceleration();
    }

    void FixedUpdate()
    {
        //условие для движения с помощью акселерометра:
        if (accelerationControl)
        {
            AccelerationMovment();
            //MovementShipAcceleration();
        }

        //условие для движения с помощью джойстика:
        if (joystickControl)
        {
            MovementShipJoystick();
        }
    }

    void AreaLimitation()
    {
        //ограничиваем зону полёта коробля, т.е. за какие пределы он не вылетит
        positionX = Mathf.Clamp(ship.position.x, xMin, xMax);
        positionZ = Mathf.Clamp(ship.position.z, zMin, zMax);
        positionY = ship.position.y;  //ось у никак не надо ограничивать поэтому пишеться так

        //теперь ограниченую позицию нужно положить обратно к кораблю
        ship.position = new Vector3(positionX, positionY, positionZ);
    }

    void CalibrateAcceleration()
    {
        //получаю данные с акселерометра и записываю в переменную:
        Vector3 accelerationSnapshot = Input.acceleration;

        //поварачиваем телефон из положения лицом вверх в положение полученое от акселерометра. Функция возвращает координаты положения телефона в пространстве типа Quaternion(т.е. положение телефона)
        Quaternion rotationPhone = Quaternion.FromToRotation(new Vector3(0, 0, -1), accelerationSnapshot);

        //инверсирую значение оси:
        callibrateRotation = Quaternion.Inverse(rotationPhone);
    }

    Vector3 FixAcceleration(Vector3 acceleration)
    {
        //умножаем стартовое положение телефона на текущее и получаем текущее положение телефона с учётом колибровки:
        Vector3 fixedAcceleration = callibrateRotation * acceleration;
        return fixedAcceleration;
    }

    void AccelerationMovment()  //метод движения через акселерометр
    {
        //проверка положения телефона в пространстве:
        Vector3 accelerationRaw = Input.acceleration;

        //получаем координаты текущего положения телефона относительно стартового положения:
        Vector3 acceleration = FixAcceleration(accelerationRaw);

        ship.rotation = Quaternion.Euler(ship.velocity.z * tilt, 0, -ship.velocity.x * tilt);  //реализация наклона по осям

        ship.velocity = new Vector3(acceleration.x, 0f, acceleration.y) * speedAcceleration;  //передвижение

        AreaLimitation();
    }

    private void MovementShipAcceleration()  //движение через акселерометр
    {
        Vector3 direction = Vector3.zero;

        direction.x = Input.acceleration.x;
        direction.z = Input.acceleration.y;

        if (direction.sqrMagnitude > 1)
            direction.Normalize();

        ship.velocity = direction * speedAcceleration;
        
        //настройка наклона коробля при поворотах и движений вперёд и назад
        ship.rotation = Quaternion.Euler(ship.velocity.z * tilt, 0, -ship.velocity.x * tilt);  //реализация наклона по осям

        AreaLimitation();
    }

    private void MovementShipJoystick()
    {
        //moveHorizontal = joystick.Horizontal * speedJoystick;  //создаётся переменная в которой определяется куда будет двигаться объект по горизонтале и с какой скоростью
        //moveVertical = joystick.Vertical * speedJoystick;  //тоже самое, но по вертикале

        if (joystick.Horizontal >= .5f)
            moveHorizontal = speedJoystick;
        else if (joystick.Horizontal <= -.5f)
            moveHorizontal = -speedJoystick;
        else if (joystick.Vertical >= .5f)
            moveVertical = speedJoystick;
        else if (joystick.Vertical <= -.5f)
            moveVertical = -speedJoystick;
        else
        {
            moveHorizontal = 0f;
            moveVertical = 0f;
        }

        ship.velocity = new Vector3(moveHorizontal, 0, moveVertical);  //задаю скорость и движение корабля

        //настройка наклона коробля при поворотах и движений вперёд и назад
        ship.rotation = Quaternion.Euler(ship.velocity.z * tilt, 0, -ship.velocity.x * tilt);  //реализация наклона по осям

        AreaLimitation();
    }

    public void MainGunShoot()  //реализация выстрела из основного орудия
    {
        if (Time.time > nextShot)  //условие следующее, если текущее время больше заданного(следующего выстрела), то можем стрелять
        {
            GameObject lazerR = Instantiate(lazerShotPrefab, lazerGunR.transform.position, Quaternion.identity);  //выстрел, используя этот метод сразу идёт тот объект которым хотим стрелять, далее задаётся позиция откуда будет производится выстрел, и его углы наклон в данном случае они не нужны
            GameObject lazerL = Instantiate(lazerShotPrefab, lazerGunL.transform.position, Quaternion.identity);
            nextShot = Time.time + shotDelay;  //переставляем время следующего выстрела

            lazerL.GetComponent<LazerScript>().Damage = damageLazer;
            lazerR.GetComponent<LazerScript>().Damage = damageLazer;
        }
    }

    public void AuxiliaryGunShoot()  //реализация выстрелов из маленьких пушек на крыльях
    {
        if (Time.time > nextShot)
        {
            GameObject lazerR = Instantiate(miniLazerShotPrefab, miniGunR.transform.position, Quaternion.identity);
            GameObject lazerL = Instantiate(miniLazerShotPrefab, miniGunL.transform.position, Quaternion.identity);
            nextShot = Time.time + shotDelay;

            lazerL.GetComponent<LazerScript>().Damage = damageAuxiliaryLazer;
            lazerR.GetComponent<LazerScript>().Damage = damageAuxiliaryLazer;
        }
    }

    //проверка на бонусы и появляется щит
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Bonus")
            shield.SetActive(true);
    }
}