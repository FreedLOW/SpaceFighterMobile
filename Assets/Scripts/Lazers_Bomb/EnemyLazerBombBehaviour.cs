using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyLazerBombBehaviour : MonoBehaviour
{
    public float speed;
    public GameObject playerExplosions;
    public GameObject bombExplosions;
    
    private GameObject boss;
    Rigidbody thisRgb;

    private void Start()
    {
        thisRgb = GetComponent<Rigidbody>();

        if(TryGetComponent(out BossScript bossScript))  //пытаюсь на сцене найти босса
            boss = FindObjectOfType<BossScript>().gameObject;
        if (boss != null)
            thisRgb.rotation = boss.transform.rotation;
    }

    private void FixedUpdate()
    {
        if (boss != null)
            transform.position += transform.forward * speed * Time.deltaTime;
        else transform.position += -transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid" || other.tag == "EnemyPlayer")
        {
            return;
        }
        else if (other.tag == "Shield")  //если на игроке щит
        {
            GameObject.FindGameObjectWithTag("Shield").SetActive(false);  //дезактивирую щит
            Instantiate(bombExplosions, transform.position, Quaternion.identity);
            Destroy(gameObject);  //уничтожаю лазерный выстрел
        }
        //если тэг того с чем столкнулся лазерный выстрел равен "игрок"
        else if (other.tag == "Player")
        {
            Instantiate(playerExplosions, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);  //чничтожаем то с чем столкнулись (игрока)
            Destroy(gameObject);  //уничтожаею лазерный выстрел
            HUD.Instance.ShowWindow(HUD.Instance.levelLoseWindow);
        }
    }
}