using UnityEngine;

public class ScrollerScript : MonoBehaviour
{
    private Vector3 startPosition;  //объявляем стартовую позицию фона

    public float speed;  //переменная для контроля скорости перемещения (скролинга) фона (бэкграунда)
    
    void Start()
    {
        startPosition = transform.position;  //запоминаем стартовую позицию
    }
    
    void Update()
    {
        //переменная для определения на сколько нужно дфигать фон
        float move = Mathf.Repeat(Time.time * speed, 200);  //матф.рэпит позволяет зациклить значение, чтобы оно не выходило за нужные границы, что и написано в строке, где 150 - макс. значение движения фона
        //написано 150, а не -150 как в юнити, потому что далее используем вектор3.бэк
        //меняем позицию фона
        transform.position = startPosition + Vector3.back * move;  //к начальной позиции добавляем что фон должен двигаться вниз и домнажаем на move который определяет премещение фона
    }
}