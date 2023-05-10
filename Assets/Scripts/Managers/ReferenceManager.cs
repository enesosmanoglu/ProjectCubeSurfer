using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ReferenceManager : MonoBehaviour
{
    #region Singleton
    public static ReferenceManager Instance { get; private set; }
    private void Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    private void Awake()
    {
        Singleton();
        FindReferences();
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => FindReferences();
    }
    public void FindReferences()
    {
        mainMenuButtons = new List<Button>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerCubes = GameObject.FindGameObjectWithTag("PlayerCubes")?.transform;
        worldCubes = GameObject.FindGameObjectWithTag("WorldCubes")?.transform;
        platform = GameObject.FindGameObjectWithTag("Platform")?.transform;
        GameObject[] finishPlatformsArray = GameObject.FindGameObjectsWithTag("Finish");
        foreach (GameObject finishPlatform in finishPlatformsArray)
        {
            finishPlatforms.Add(finishPlatform.transform);
        }

        scoreText = GameObject.FindGameObjectWithTag("ScoreText")?.GetComponent<TMP_Text>();
        cubeText = GameObject.FindGameObjectWithTag("CubeText")?.GetComponent<TMP_Text>();
        levelText = GameObject.FindGameObjectWithTag("LevelText")?.GetComponent<TMP_Text>();
        nextLevelText = GameObject.FindGameObjectWithTag("NextLevelText")?.GetComponent<TMP_Text>();

        startButton = GameObject.FindGameObjectWithTag("StartButton")?.GetComponent<Button>();
        exitButton = GameObject.FindGameObjectWithTag("ExitButton")?.GetComponent<Button>();
        pauseButton = GameObject.FindGameObjectWithTag("PauseButton")?.GetComponent<Button>();
        resumeButton = GameObject.FindGameObjectWithTag("ResumeButton")?.GetComponent<Button>();
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("MainMenuButton");
        foreach (GameObject button in buttons)
        {
            mainMenuButtons.Add(button.GetComponent<Button>());
        }
        nextLevelButton = GameObject.FindGameObjectWithTag("NextLevelButton")?.GetComponent<Button>();
        restartButton = GameObject.FindGameObjectWithTag("RestartButton")?.GetComponent<Button>();
        settingsButton = GameObject.FindGameObjectWithTag("SettingsButton")?.GetComponent<Button>();

        pauseCanvas = GameObject.FindGameObjectWithTag("PauseCanvas")?.GetComponent<Canvas>();
        endLevelCanvas = GameObject.FindGameObjectWithTag("EndLevelCanvas")?.GetComponent<Canvas>();

        mainMenuPanel = GameObject.FindGameObjectWithTag("MainMenuPanel");
        settingsPanel = GameObject.FindGameObjectWithTag("SettingsPanel");

        effectsSlider = GameObject.FindGameObjectWithTag("EffectsSlider")?.GetComponent<Slider>();
        musicSlider = GameObject.FindGameObjectWithTag("MusicSlider")?.GetComponent<Slider>();
    }

    [Header("Transforms")]
    public Transform player;
    public Transform playerCubes;
    public Transform worldCubes;
    public Transform platform;
    public List<Transform> finishPlatforms;

    [Header("UI Texts")]
    public TMP_Text scoreText;
    public TMP_Text cubeText;
    public TMP_Text levelText;
    public TMP_Text nextLevelText;

    [Header("UI Buttons")]
    public Button startButton;
    public Button exitButton;
    public Button pauseButton;
    public Button resumeButton;
    public List<Button> mainMenuButtons;
    public Button nextLevelButton;
    public Button restartButton;
    public Button settingsButton;

    [Header("UI Canvases")]
    public Canvas pauseCanvas;
    public Canvas endLevelCanvas;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("UI Sliders")]
    public Slider effectsSlider;
    public Slider musicSlider;

}
