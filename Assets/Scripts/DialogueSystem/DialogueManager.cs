using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;  //тут хранится поле текста диалога

    public Dialogue dialogue;

    private Queue<string> sentences;  //создаю очередь(Queue) из предложений

    public Button controllButton;
    private TextMeshProUGUI buttonText;

    public Animator loadAnim;

    private void Start()
    {
        GameController.Instance.State = GameState.Play;

        sentences = new Queue<string>();  //создаю экземпляр класса Queue

        buttonText = controllButton.GetComponent<TextMeshProUGUI>();

        StartDialogue(dialogue);
    }

    public void StartDialogue(Dialogue dialogue)  //метод запуска диалога:
    {
        sentences.Clear();  //очищаю предложения

        foreach(string sentence in dialogue.sentences)  //перебираю массив предложений
        {
            sentences.Enqueue(sentence);  //добавляю найденное предложение в конец очереди
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()  //метод перехода к следующему предложению:
    {
        if (sentences.Count == 0)  //если предложений в диалоге нету, то:
        {
            StartCoroutine(EndDialogues());
            return;
        }
        else if (sentences.Count == 2)  //если предложений осталось 2, то:
            dialogueText.alignment = TextAlignmentOptions.TopLeft;  //выравнивание текста ставлю по верхнему левому краю
        else if (sentences.Count == 1)  //если осталось 1 предложение, то:
        {
            dialogueText.alignment = TextAlignmentOptions.TopRight;  //выравниваю текст предложения по верхнему правому краю
            controllButton.GetComponentInChildren<TMP_Text>().text = "Play";  //нахожу у ребёнка кнопки компонент текст и присваиваю ему новое значение
        }

        string sentence = sentences.Dequeue();  //очищаю очередь в предложениях диалога с помощью метода - Dequeue

        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));  //запускаю корутину "писать предложение" в качестве аргумента передаётся предложение в очереди
    }

    public void DisplayNextSentenceDialogue2()  //метод для перехожа к следующему предложению во время 2-го диалога:
    {
        if (sentences.Count == 0)  //если предложений в диалоге нету, то:
        {
            StartCoroutine(EndDialogues());
            return;
        }
        else if (sentences.Count == 3)  //если предложений осталось 2, то:
            dialogueText.alignment = TextAlignmentOptions.TopRight;  //выравнивание текста ставлю по верхнему левому краю
        else if (sentences.Count == 2)  //если осталось 1 предложение, то:
            dialogueText.alignment = TextAlignmentOptions.TopLeft;  //выравниваю текст предложения по верхнему правому краю
        else if (sentences.Count == 1)
        {
            dialogueText.alignment = TextAlignmentOptions.TopRight;
            controllButton.GetComponentInChildren<TMP_Text>().text = "Play";  //нахожу у ребёнка кнопки компонент текст и присваиваю ему новое значение
        }

        string sentence = sentences.Dequeue();  //очищаю очередь в предложениях диалога с помощью метода - Dequeue

        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));  //запускаю корутину "писать предложение" в качестве аргумента передаётся предложение в очереди
    }

    public void DisplayNextSentenceDialogue3()  //метод для перехожа к следующему предложению во время 3-го диалога:
    {
        if (sentences.Count == 0)  //если предложений в диалоге нету, то:
        {
            StartCoroutine(EndDialogues());
            return;
        }
        else if (sentences.Count == 4)  //если предложений осталось 2, то:
            dialogueText.alignment = TextAlignmentOptions.TopRight;  //выравнивание текста ставлю по верхнему левому краю
        else if (sentences.Count == 3)  //если осталось 1 предложение, то:
            dialogueText.alignment = TextAlignmentOptions.TopLeft;  //выравниваю текст предложения по верхнему правому краю
        else if (sentences.Count == 2)
            dialogueText.alignment = TextAlignmentOptions.TopRight;
        else if (sentences.Count == 1)
        {
            dialogueText.alignment = TextAlignmentOptions.TopLeft;
            controllButton.GetComponentInChildren<TMP_Text>().text = "Play";  //нахожу у ребёнка кнопки компонент текст и присваиваю ему новое значение
        }

        string sentence = sentences.Dequeue();  //очищаю очередь в предложениях диалога с помощью метода - Dequeue

        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));  //запускаю корутину "писать предложение" в качестве аргумента передаётся предложение в очереди
    }

    //создаю корутину в которой реализавана "анимация" предложений:
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";  //задаю текст диалога

        foreach(char letter in sentence.ToCharArray())  //перебираю символы в предложениях диалога конвертируя их в массив символов
        {
            dialogueText.text += letter;  //добавляю каждую букву
            yield return new WaitForSeconds(.03f);  //никакой задержки не делаю
        }
    }

    IEnumerator EndDialogues()  //корутина конца диалога, будет вызываться если персонажи высказали все свои предложения:
    {
        loadAnim.SetTrigger("ToGame");

        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}