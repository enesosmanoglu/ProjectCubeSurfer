using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ReferenceManager : MonoBehaviour
{
    #region Singleton Bool
    public static ReferenceManager Instance { get; private set; }
    private bool Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            Destroy(this);
            return false;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
    }
    #endregion

    private void Awake()
    {
        if (!Singleton()) return;
        Debug.Log("### AWAKE ### ReferenceManager");
        FindReferences();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("### SCENE LOADED ### ReferenceManager >> " + scene.name);
        FindReferences();
    }
    private void OnEnable()
    {
        Debug.Log("### ENABLED ### ReferenceManager");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        Debug.Log("### DISABLED ### ReferenceManager");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void FindReferences()
    {
        #region Transforms
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerCubes = GameObject.FindGameObjectWithTag("PlayerCubes")?.transform;
        worldCubes = GameObject.FindGameObjectWithTag("WorldCubes")?.transform;
        platform = GameObject.FindGameObjectWithTag("Platform")?.transform;
        #endregion

        finishPlatforms = new List<Transform>();
        GameObject[] finishPlatformsArray = GameObject.FindGameObjectsWithTag("Finish");
        foreach (GameObject finishPlatform in finishPlatformsArray)
        {
            finishPlatforms.Add(finishPlatform.transform);
        }

        #region Texts
        scoreText = GameObject.FindGameObjectWithTag("ScoreText")?.GetComponent<TMP_Text>();
        cubeText = GameObject.FindGameObjectWithTag("CubeText")?.GetComponent<TMP_Text>();
        levelText = GameObject.FindGameObjectWithTag("LevelText")?.GetComponent<TMP_Text>();
        nextLevelText = GameObject.FindGameObjectWithTag("NextLevelText")?.GetComponent<TMP_Text>();
        #endregion

        #region Buttons
        startButton = GameObject.FindGameObjectWithTag("StartButton")?.GetComponent<Button>();
        exitButton = GameObject.FindGameObjectWithTag("ExitButton")?.GetComponent<Button>();
        pauseButton = GameObject.FindGameObjectWithTag("PauseButton")?.GetComponent<Button>();
        resumeButton = GameObject.FindGameObjectWithTag("ResumeButton")?.GetComponent<Button>();
        nextLevelButton = GameObject.FindGameObjectWithTag("NextLevelButton")?.GetComponent<Button>();
        restartButton = GameObject.FindGameObjectWithTag("RestartButton")?.GetComponent<Button>();
        settingsButton = GameObject.FindGameObjectWithTag("SettingsButton")?.GetComponent<Button>();
        rewardButton = GameObject.FindGameObjectWithTag("RewardButton")?.GetComponent<Button>();
        marketButton = GameObject.FindGameObjectWithTag("MarketButton")?.GetComponent<Button>();
        removeAdsButton = GameObject.FindGameObjectWithTag("RemoveAdsButton")?.GetComponent<Button>();
        restorePurchasesButton = GameObject.FindGameObjectWithTag("RestorePurchasesButton")?.GetComponent<Button>();
        #endregion

        mainMenuButtons = new List<Button>();
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("MainMenuButton");
        foreach (GameObject button in buttons)
        {
            mainMenuButtons.Add(button.GetComponent<Button>());
        }

        #region Canvases
        pauseCanvas = GameObject.FindGameObjectWithTag("PauseCanvas")?.GetComponent<Canvas>();
        endLevelCanvas = GameObject.FindGameObjectWithTag("EndLevelCanvas")?.GetComponent<Canvas>();
        #endregion

        #region Panels
        mainMenuPanel = GameObject.FindGameObjectWithTag("MainMenuPanel");
        settingsPanel = GameObject.FindGameObjectWithTag("SettingsPanel");
        marketPanel = GameObject.FindGameObjectWithTag("MarketPanel");
        #endregion

        #region Sliders
        effectsSlider = GameObject.FindGameObjectWithTag("EffectsSlider")?.GetComponent<Slider>();
        musicSlider = GameObject.FindGameObjectWithTag("MusicSlider")?.GetComponent<Slider>();
        #endregion

        #region Other
        nativeAd = GameObject.FindGameObjectWithTag("NativeAd");
        #endregion

        #region Cube Skin Buttons
        cubeSkinButtons = new List<Button>();
        cubeSkinButtonsPriced = new List<Button>();
        GameObject buttonsParent = GameObject.FindGameObjectWithTag("CubeSkinPricedButton")?.transform.parent.gameObject;
        if (buttonsParent != null)
        {
            int i = 0;
            foreach (Button button in buttonsParent.GetComponentsInChildren<Button>())
            {
                bool priced = button.tag == "CubeSkinPricedButton";
                // Debug.Log("i: " + i + " button: " + button.name + " priced: " + priced);
                if (priced)
                    cubeSkinButtonsPriced.Add(button);
                else
                    cubeSkinButtons.Add(button);
                i++;
            }
        }
        #endregion 
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
    public Button rewardButton;
    public Button marketButton;
    public Button removeAdsButton;
    public List<Button> cubeSkinButtons;
    public List<Button> cubeSkinButtonsPriced;
    public Button restorePurchasesButton;

    [Header("UI Canvases")]
    public Canvas pauseCanvas;
    public Canvas endLevelCanvas;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject marketPanel;

    [Header("UI Sliders")]
    public Slider effectsSlider;
    public Slider musicSlider;

    [Header("Other")]
    public GameObject nativeAd;

}
