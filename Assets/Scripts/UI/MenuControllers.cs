using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuControllers : MonoBehaviour
{
    public AudioMixer audioMixer;

    public GameObject mainMenu;

    public GameObject howToPlay;

    public GameObject settingsMenu;

    public Slider m_MusicSlider;  //тут хранится слайдер регулировки громкости музыки

    public Slider m_SoundSlider;  //тут хранится слайдер регулировки громкости звуков

    public Toggle joystickToggle;

    public Toggle accelerationToggle;

    Resolution[] resolutions;  //массив в котором хранятся все возможные разрешения экранов

    static private MenuControllers _instance;

    public static MenuControllers Instance { get => _instance; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        
        GameController.Instance.State = GameState.Play;

        //SetResolutions();

        if (GameController.Instance.loadSceneFirstTime == false)
        {
            //загружаю сохранённые значения громкости:
            m_SoundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
            m_MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (joystickToggle.isOn)
        {
            PlayerSpaceSript.joystickControl = true;
        }
    }

    void SetResolutions()  //метод для определения разрешения экрана
    {
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
    }

    public void ControlJoystick(bool controlJoystick)
    {
        controlJoystick = joystickToggle.isOn;

        if (controlJoystick)
        {
            GameController.Instance.ControlManagement = ControlManagement.Joystick;
           
            accelerationToggle.isOn = false;
        }
    }

    public void ControlAceleration(bool controlAceleration)
    {
        controlAceleration = accelerationToggle.isOn;

        if (controlAceleration)
        {
            GameController.Instance.ControlManagement = ControlManagement.Acceleration;

            joystickToggle.isOn = false;
        }
    }

    public void HowToPlayPressed()
    {
        mainMenu.SetActive(false);

        howToPlay.SetActive(true);
    }

    //метод для загрузки сцены игры
    //За загрузку сцены отвечает SceneManager и у него есть метод LoadScene который я и использую и передаю следующее: взять текущюю сцену и прибавть 1, т.е. будет следующая сцена
    public void PlayPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //метод для конпки настройки
    public void SettingsPressed()
    {
        mainMenu.SetActive(false);

        settingsMenu.SetActive(true);
    }

    public void BackToMenuPressed()
    {
        settingsMenu.SetActive(false);

        howToPlay.SetActive(false);

        mainMenu.SetActive(true);
    }

    //метод для выхода из сцены
    public void ExitPressed()
    {
        Application.Quit();
    }

    //метод настройки громкости музыки:
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("m_volume", volume);
        
        PlayerPrefs.SetFloat("MusicVolume", volume);  //сохраняю значение ползунка громкости для перехода между сценами

        GameController.Instance.loadSceneFirstTime = false;
    }

    //метод настройки громкости звуков в игре:
    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("s_volume", volume);

        PlayerPrefs.SetFloat("SoundVolume", volume);

        GameController.Instance.loadSceneFirstTime = false;
    }
}