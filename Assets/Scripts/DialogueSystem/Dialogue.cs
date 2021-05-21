using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [TextArea(3, 10)]  //задаю поле текста, минимум 3 предлжения, а максимум 10
    public string[] sentences;  //создаю массив предложений в котором будут хранится предложения разговора между персонажами
}