using UnityEngine;

//энумератор для контроля состояния игры
public enum GameState { Play, Pause}

public class GameController : MonoBehaviour
{
    private int score;

    private GameState state;  //контроль игрового состояния

    private static GameController m_instance;

    public int Score { get => score; set => score = value; }  //свойство для обновления получения очков

    public bool loadSceneFirstTime = true;

    public static GameController Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject controller = Instantiate(Resources.Load("Prefabs/GameController/GameController")) as GameObject;

                m_instance = controller.GetComponent<GameController>();
            }
            return m_instance;
        }
    }

    public GameState State 
    { 
        get => state; 
        set 
        {
            if (value == GameState.Play)
            {
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 0.0f;
            }
            state = value;
        } 
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            if (m_instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);

        State = GameState.Play;
        InitializeAudioManager();
    }

    private void InitializeAudioManager()
    {
        gameObject.AddComponent<AudioListener>();
    }
}