using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton Bool
    public static UIManager Instance { get; private set; }
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
        Debug.Log("### AWAKE ### UIManager");
        AddListeners();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("### SCENE LOADED ### UIManager >> " + scene.name);
        AddListeners();
    }

    private void OnEnable()
    {
        Debug.Log("### ENABLED ### UIManager");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Debug.Log("### DISABLED ### UIManager");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AddListeners()
    {
        Managers.Reference.startButton?.onClick.RemoveAllListeners();
        Managers.Reference.startButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();

            int level = PlayerPrefs.GetInt("Level", 1);
            string levelName = "Level" + level.ToString();

            List<string> scenesInBuild = new List<string>();
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }

            if (scenesInBuild.Contains(levelName))
                SceneManager.LoadScene(levelName);
            else
            {
                Debug.Log(levelName + " not found, loading Level1");
                SceneManager.LoadScene("Level1");
            }

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
            if (Managers.Ad.adsRemoved)
                Managers.Reference.nativeAd?.gameObject.SetActive(false);
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
            Managers.Sound.SetVolumeEffects(value);
        });

        Managers.Reference.musicSlider?.onValueChanged.RemoveAllListeners();
        Managers.Reference.musicSlider?.onValueChanged.AddListener((float value) =>
        {
            Managers.Sound.SetVolumeMusic(value);
        });

        Managers.Reference.rewardButton?.onClick.RemoveAllListeners();
        Managers.Reference.rewardButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            Managers.Ad.ShowRewardedAd();
        });

        Managers.Reference.marketButton?.onClick.RemoveAllListeners();
        Managers.Reference.marketButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            Managers.Reference.mainMenuPanel.SetActive(false);
            Managers.Reference.marketPanel.SetActive(true);
            if (Managers.Ad.adsRemoved)
                Managers.Reference.removeAdsButton?.gameObject.SetActive(false);
            for (int i = 0; i < Managers.Skin.cubeMaterialsPriced.Length; i++)
            {
                string name = Managers.Skin.cubeMaterialsPriced[i].name;
                if (PlayerPrefs.GetInt("purchased_" + name, 0) == 1)
                {
                    Managers.Reference.cubeSkinButtonsPriced[i]?.transform.GetChild(1)?.gameObject.SetActive(false);
                }
            }
            SetCubeSkinButtonColors();
        });

        Managers.Reference.removeAdsButton?.onClick.RemoveAllListeners();
        Managers.Reference.removeAdsButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            Managers.IAP.PurchaseProduct("removeads");
            // Managers.Reference.removeAdsButton?.gameObject.SetActive(false);
            // Managers.Ad.RemoveAds();
        });

        Managers.Reference.restorePurchasesButton?.onClick.RemoveAllListeners();
        Managers.Reference.restorePurchasesButton?.onClick.AddListener(() =>
        {
            Managers.Sound.PlayButtonClickSound();
            Managers.IAP.RestoreProducts();
        });

        foreach (var button in Managers.Reference.cubeSkinButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                Managers.Sound.PlayButtonClickSound();
                int index = button.transform.GetSiblingIndex();
                Managers.Skin.SetCubeSkin(index);
                SetCubeSkinButtonColors(button);
            });
        }

        foreach (var button in Managers.Reference.cubeSkinButtonsPriced)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                Managers.Sound.PlayButtonClickSound();
                int index = button.transform.GetSiblingIndex() - Managers.Reference.cubeSkinButtons.Count;
                Debug.Log("index: " + index);
                string priceText = Managers.Skin.cubePrices[index];
                Debug.Log("priceText: " + priceText);
                string type = priceText.Substring(0, 1);
                Debug.Log("type: " + type);
                int price = Int32.Parse(priceText.Substring(1));
                Debug.Log("price: " + price);
                string name = Managers.Skin.cubeMaterialsPriced[index].name;
                Debug.Log("name: " + name);

                if (type == "d")
                {
                    bool purchased = PlayerPrefs.GetInt("purchased_" + name, 0) == 1;
                    Debug.Log("purchased: " + purchased);
                    if (purchased)
                    {
                        Managers.Skin.SetCubeSkinPriced(index);
                        SetCubeSkinButtonColors(button);
                        return;
                    }
                    if (Managers.Game.diamonds >= price)
                    {
                        Managers.Game.DecreaseDiamond(price);
                        Managers.Skin.SetCubeSkinPriced(index);
                        SetCubeSkinButtonColors(button);
                        PlayerPrefs.SetInt("purchased_" + name, 1);
                        Managers.Reference.cubeSkinButtonsPriced[index]?.transform.GetChild(1)?.gameObject.SetActive(false);
                    }
                    else
                    {
                        // TODO: show not enough diamonds
                    }

                }
                else if (type == "i")
                {
                    bool purchased = PlayerPrefs.GetInt("purchased_" + name, 0) == 1;
                    Debug.Log("purchased: " + purchased);
                    if (purchased)
                    {
                        Managers.Skin.SetCubeSkinPriced(index);
                        SetCubeSkinButtonColors(button);
                        return;
                    }
                    Managers.IAP.PurchaseProduct(name);
                }
            });
        }

    }

    public void SetCubeSkinButtonColors(Button button = null)
    {
        if (button == null)
        {
            string currentCubeMaterialName = Managers.Skin.currentCubeMaterial.name;
            button = Managers.Reference.cubeSkinButtons.Find(b => b.name == currentCubeMaterialName) ?? Managers.Reference.cubeSkinButtonsPriced.Find(b => b.name == currentCubeMaterialName);
        }
        button.GetComponent<Image>().color = Color.green;
        foreach (var otherButton in Managers.Reference.cubeSkinButtons)
        {
            if (otherButton != button && otherButton.GetComponent<Image>().color == Color.green)
                otherButton.GetComponent<Image>().color = Color.white;
        }
        foreach (var otherButton in Managers.Reference.cubeSkinButtonsPriced)
        {
            if (otherButton != button && otherButton.GetComponent<Image>().color == Color.green)
                otherButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void UpdateScoreText()
    {
        Managers.Reference.scoreText.text = "<color=purple>♦</color> " + Managers.Game.diamonds.ToString();
    }

    public void UpdateCubeText()
    {
        Managers.Reference.cubeText.text = "<color=lightblue>▪</color> " + Managers.Game.GetCubeCount().ToString();
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        GUI.skin.button.fontSize = 30;
        GUI.skin.label.fontSize = 30;
        //Delete all of the PlayerPrefs settings by pressing this button.
        if (GUI.Button(new Rect(20, 20, 350, 60), "Delete Player Prefs"))
        {
            PlayerPrefs.DeleteAll();
            Managers.Ad.adsRemoved = false;
        }
        if (GUI.Button(new Rect(20, 100, 350, 60), "Add 1000 Diamonds"))
        {
            Managers.Game.CollectDiamond(1000);
        }
        if (GUI.Button(new Rect(380, 20, 350, 60), "Level UP"))
        {
            int level = PlayerPrefs.GetInt("Level", 1);
            PlayerPrefs.SetInt("Level", level + 1);
        }
        if (GUI.Button(new Rect(380, 100, 350, 60), "Level DOWN"))
        {
            int level = PlayerPrefs.GetInt("Level", 1);
            PlayerPrefs.SetInt("Level", level - 1);
        }
        GUI.Label(new Rect(20, 260, 350, 60), "Ads Removed: " + Managers.Ad.adsRemoved.ToString());
        GUI.Label(new Rect(20, 340, 350, 60), "Level: " + PlayerPrefs.GetInt("Level", 1).ToString());

        if (!Managers.Ad.adsRemoved)
            if (GUI.Button(new Rect(20, 180, 350, 60), "Remove Ads"))
            {
                Managers.Ad.RemoveAds();
            }
    }
#endif
}
