using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour, IDestructable
{
    public GameObject[] gun;
    public GameObject gunBomb;

    private bool seeTarget;

    private NavMeshAgent _navmeshagant;

    private Transform target;

    public float shotDelay;  //задержка между выстрелами

    private float nextShot;  //переменная для определения времени между выстрелами

    public GameObject lazerShot;
    public GameObject bombShoot;

    public GameObject bossExplosions;

    private bool checkShoot = true;

    public float hitPoints;  //колличество жизни
    private float hitPointsCurrent;  //текущее колличество жизней

    private void Start()
    {
        hitPointsCurrent = hitPoints;  //приравнюю колличество к текущему

        _navmeshagant = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerSpaceSript>().gameObject.transform;

        if (checkShoot == true)
        {
            StartCoroutine(FirstShoot());
        }

        if (gunBomb != null)
            InvokeRepeating("BossShootBomb", 12f, 6f);
    }

    private void Update()
    {
        if (target != null)
        {
            _navmeshagant.SetDestination(target.position);
        }
        
        CheckTarget();

        if (checkShoot == false)
            ShotBoss();
    }

    public void ShotBoss()  //метод в котором реализован выстрел босса с определённой задержкой
    {
        if (seeTarget == true && Time.time > nextShot)
        {
            for (int i = 0; i < gun.Length; i++)
            {
                Instantiate(lazerShot, gun[i].transform.position, Quaternion.identity);
            }

            nextShot = Time.time + shotDelay;  //переставляем время следующего выстрела
        }
    }

    IEnumerator FirstShoot()
    {
        yield return new WaitForSeconds(5f);

        for (int i = 0; i < gun.Length; i++)
        {
            Instantiate(lazerShot, gun[i].transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(3f);

        checkShoot = false;
    }

    void BossShootBomb()
    {
        Instantiate(bombShoot, gunBomb.transform.position, Quaternion.identity);
    }

    public void CheckTarget()
    {
        for (int i = 0; i < gun.Length; i++)
        {
            if (target != null)
            {
                Vector3 targetDirection = target.position - gun[i].transform.position;

                Ray ray = new Ray(gun[i].transform.position, targetDirection);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == target)
                    {
                        seeTarget = true;
                        return;
                    }
                }
                seeTarget = false;
            }
        }
    }

    public void Hit(float damage)
    {
        hitPointsCurrent -= damage;   //уменьшаю текущее колличество жизней

        if (hitPointsCurrent <= 0)  //если текущее значение жизни меньше или равно нулю, то:
        {
            Destroy(gameObject);  //чничтожаю объект на котором этот скрипт
            HUD.Instance.UpdateScore(50);  //добавляю 50 очков
            HUD.Instance.ShowWindow(HUD.Instance.levelCompletedWindow);  //открываю окно что уровень пройден
        }
    }
}