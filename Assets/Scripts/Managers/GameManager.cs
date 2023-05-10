using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
// using TMPro;
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
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
        Camera = Camera.main;
        cameraStartOffset = Camera.transform.position - Managers.Reference.player.position;
        cameraStartRotation = Camera.transform.rotation;
        // set camera on scene load
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            Camera = Camera.main;
            // set camera pos back to player
            Camera.transform.position = Managers.Reference.player.position + cameraStartOffset;
            Camera.transform.rotation = cameraStartRotation;

            Managers.Reference.pauseButton?.gameObject.SetActive(true);
            Managers.Reference.pauseCanvas?.gameObject.SetActive(false);
            Managers.Reference.settingsPanel?.SetActive(false);
            Managers.Reference.effectsSlider?.SetValueWithoutNotify(Managers.Sound.volume);
            Managers.Reference.musicSlider?.SetValueWithoutNotify(Managers.Sound.bgVolume);
            Managers.Reference.endLevelCanvas?.gameObject.SetActive(false);
            Start();
            Managers.UI.UpdateCubeText();
            Managers.UI.UpdateScoreText();
        };
        diamonds = PlayerPrefs.GetInt("Diamonds", 0);
        Managers.UI.UpdateScoreText();
    }

    private void Start()
    {
        playerStartPos = Managers.Reference.player.position;
        cubeCount = startCubeCount;
        for (int i = 0; i < Managers.Reference.playerCubes.childCount; i++)
        {
            Transform child = Managers.Reference.playerCubes.GetChild(i);
            if (child.CompareTag("Cube"))
                Destroy(child.gameObject);
        }
        for (int i = 0; i < startCubeCount; i++)
        {
            GameObject cube = Instantiate(Managers.Prefab.cubePrefab, Managers.Reference.playerCubes);
            cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            cube.transform.localPosition = new Vector3(0, i, 0);
            if (i == startCubeCount - 1)
            {
                Managers.Reference.player.localPosition = new Vector3(0, i + 2, 0);
                Managers.Reference.player.SetParent(cube.transform);
            }
        }

        CreateEnvironment();
    }

    private void CreateEnvironment()
    {
        if (Managers.Reference.finishPlatforms.Count == 0)
        {
            Debug.LogError("No finish platforms found!");
            return;
        }

        Vector3 startPos = playerStartPos;
        startPos.y = Managers.Reference.platform.position.y;

        Vector3 endPos = Managers.Reference.finishPlatforms[Managers.Reference.finishPlatforms.Count - 1].position;
        endPos.y = Managers.Reference.platform.position.y;

        float[] xPositions = { Managers.Reference.platform.position.x - Managers.Game.platformWidth, Managers.Reference.platform.position.x + Managers.Game.platformWidth };

        for (int i = 0; i < xPositions.Length; i++)
        {
            float x = xPositions[i];
            startPos.x = x;
            endPos.x = x;

            Vector3 pos = startPos;
            while (pos.z < endPos.z)
            {
                GameObject cube = Instantiate(Managers.Prefab.wallCubeRotatingPrefab, Managers.Reference.worldCubes);
                cube.transform.position = pos;
                pos.z += 5;
            }
        }
    }

    private void Update()
    {
        if (gameState == GameState.Paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void GameOver()
    {
        Managers.Sound.PlayGameOverSound(Managers.Reference.player);
        ResetPlayerModel();
        Managers.Reference.player.GetComponentInChildren<Animator>().SetTrigger("Die");
        if (gameState == GameState.MainMenu) return;
        gameState = GameState.GameOver;
        Managers.Reference.nextLevelButton.GetComponentInChildren<TMP_Text>().text = "RESTART";
        Managers.Reference.nextLevelText.text = "YOU LOSE";
        Managers.Reference.nextLevelText.color = Color.red;
        Managers.Reference.endLevelCanvas.gameObject.SetActive(true);
        Managers.Reference.pauseButton.gameObject.SetActive(false);
    }

    public void LevelPassed()
    {
        Managers.Sound.PlayLevelPassedSound(Managers.Reference.player);
        ResetPlayerModel();
        Managers.Reference.player.GetComponentInChildren<Animator>().SetTrigger("Win");
        gameState = GameState.LevelPassed;
        Managers.Reference.nextLevelButton.GetComponentInChildren<TMP_Text>().text = "NEXT LEVEL";
        Managers.Reference.nextLevelText.text = "YOU WIN";
        Managers.Reference.nextLevelText.color = Color.green;
        Managers.Reference.endLevelCanvas.gameObject.SetActive(true);
        Managers.Reference.pauseButton.gameObject.SetActive(false);
    }

    public void CollectDiamond()
    {
        Managers.Sound.PlayDiamondPickupSound();
        diamonds++;
        Managers.UI.UpdateScoreText();
        PlayerPrefs.SetInt("Diamonds", diamonds);
    }
    public void CollectCube(Transform cube)
    {
        Managers.Sound.PlayCubePickupSound(cube);
        Transform playerCubes = Managers.Reference.playerCubes;

        Managers.Reference.player.SetParent(playerCubes);
        cubeCount = playerCubes.childCount - 1;
        Managers.UI.UpdateCubeText();
        Debug.Log("Cube Count: " + cubeCount);

        Transform topCube = null;
        for (int i = 0; i < playerCubes.childCount; i++)
        {
            Transform child = playerCubes.GetChild(playerCubes.childCount - 1 - i);
            if (child.CompareTag("Cube"))
            {
                topCube = child;
                child.localPosition = new Vector3(0, (i + 1) * 1.1f, 0);
            }
        }

        cube.SetParent(playerCubes);
        cube.localPosition = Vector3.zero;
        cube.localRotation = Quaternion.identity;
        cube.localScale = Vector3.one;

        if (!topCube)
            topCube = cube;

        Managers.Reference.player.SetParent(topCube);
        Managers.Reference.player.localPosition = Vector3.up * 2;
        ResetPlayerModel();
        Managers.Reference.player.GetComponentInChildren<Animator>().SetTrigger("Fall");
    }

    public void ResetPlayerModel()
    {
        Managers.Reference.player.GetChild(0).localPosition = Vector3.down * 0.5f;
        Managers.Reference.player.GetChild(0).localRotation = Quaternion.identity;
    }

    [Header("Global Settings")]
    [Range(0, 10)] public int startCubeCount = 1;
    [Range(0, 16)] public float playerForwardSpeed = 7f;
    [Range(0, 10)] public float playerHorizontalSpeed = 1f;
    [Range(3, 10)] public int platformWidth = 5;
    public Vector2 diamondRotationSpeedRange = new Vector2(50, 100);


    [Header("Will be assigned automatically at runtime")]
    public GameState gameState = GameState.MainMenu;
    public int diamonds = 0;
    public int cubeCount = 0;
    public Camera Camera;

    public Vector3 cameraStartOffset = Vector3.zero;
    public Quaternion cameraStartRotation = Quaternion.identity;

    private Vector3 playerStartPos = Vector3.zero;

}

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    LevelPassed,
    GameOver,
}