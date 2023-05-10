using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { get; private set; }
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
        AddListeners();
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => AddListeners();
    }

    public void AddListeners()
    {
        Managers.Reference.startButton?.onClick.RemoveAllListeners();
        Managers.Reference.startButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            SceneManager.LoadScene("Level1");
            Managers.Game.gameState = GameState.Playing;
        });

        Managers.Reference.exitButton?.onClick.RemoveAllListeners();
        Managers.Reference.exitButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            Application.Quit();
        });

        Managers.Reference.pauseButton?.onClick.RemoveAllListeners();
        Managers.Reference.pauseButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            Managers.Game.gameState = GameState.Paused;
            Managers.Reference.pauseCanvas.gameObject.SetActive(true);
            Managers.Reference.pauseButton.gameObject.SetActive(false);
        });

        Managers.Reference.resumeButton?.onClick.RemoveAllListeners();
        Managers.Reference.resumeButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            Managers.Game.gameState = GameState.Playing;
            Managers.Reference.pauseCanvas.gameObject.SetActive(false);
            Managers.Reference.pauseButton.gameObject.SetActive(true);
        });

        for (int i = 0; i < Managers.Reference.mainMenuButtons.Count; i++)
        {
            Managers.Reference.mainMenuButtons[i]?.onClick.RemoveAllListeners();
            Managers.Reference.mainMenuButtons[i]?.onClick.AddListener(() =>
            {
                Managers.Sound.PlayButtonClickSound();
                SceneManager.LoadScene(0);
                Managers.Game.gameState = GameState.MainMenu;
            });
        }

        Managers.Reference.nextLevelButton?.onClick.RemoveAllListeners();
        Managers.Reference.nextLevelButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            if (Managers.Game.gameState == GameState.GameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Managers.Game.gameState = GameState.Playing;
                return;
            }
            int levelCount = SceneManager.sceneCountInBuildSettings - 1;
            int loadSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (loadSceneIndex > levelCount)
            {
                loadSceneIndex = 1;
            }
            SceneManager.LoadScene(loadSceneIndex);
            Managers.Game.gameState = GameState.Playing;
        });

        Managers.Reference.restartButton?.onClick.RemoveAllListeners();
        Managers.Reference.restartButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Managers.Game.gameState = GameState.Playing;
        });

        Managers.Reference.settingsButton?.onClick.RemoveAllListeners();
        Managers.Reference.settingsButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            Managers.Reference.mainMenuPanel.SetActive(false);
            Managers.Reference.settingsPanel.SetActive(true);
        });

        Managers.Reference.effectsSlider?.onValueChanged.RemoveAllListeners();
        Managers.Reference.effectsSlider?.onValueChanged.AddListener((float value) =>
        {
            Managers.Sound.volume = value;
        });

        Managers.Reference.musicSlider?.onValueChanged.RemoveAllListeners();
        Managers.Reference.musicSlider?.onValueChanged.AddListener((float value) =>
        {
            Managers.Sound.bgVolume = value;
        });
    }

    public void UpdateScoreText()
    {
        Managers.Reference.scoreText.text = "<color=purple>♦</color> " + Managers.Game.diamonds.ToString();
    }

    public void UpdateCubeText()
    {
        int childCount = Managers.Reference.playerCubes.childCount;
        if (childCount == 1 && Managers.Reference.playerCubes.GetChild(0).CompareTag("Player"))
        {
            childCount = 0;
        }
        Managers.Reference.cubeText.text = "<color=lightblue>▪</color> " + childCount.ToString();
    }
}
