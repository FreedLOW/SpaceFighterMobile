using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    //этот метод будез вызван, когда один объект (с коллайдером) покинет границу другого-текущего объекта (тоже с коллайдером)
    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);  //уничтожает всё, что покидает границу, которая построенная в юнити
    }
}