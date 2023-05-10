using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    #region Singleton
    public static MainCamera Instance { get; private set; }
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
    }

    public float zoomSpeed = 2f;
    public float zoomHeight = 0.005f;

    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Managers.Game.gameState == GameState.LevelPassed)
        {
            Vector3 target = Managers.Reference.player.position;
            target.y -= 2f;
            transform.LookAt(target);
            transform.Translate(Vector3.right * Time.deltaTime);
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + Mathf.Sin(Time.time * zoomSpeed) * zoomHeight,
                transform.position.z
            );
            return;
        }

        Managers.Game.Camera.transform.position = Vector3.Lerp(
            Managers.Game.Camera.transform.position,
            new Vector3(
                Managers.Game.Camera.transform.position.x,
                Managers.Reference.playerCubes.position.y + Managers.Game.cameraStartOffset.y + Managers.Game.cubeCount,
                Managers.Reference.playerCubes.position.z + Managers.Game.cameraStartOffset.z - Managers.Game.cubeCount
            ),
            0.1f
        );
    }
}
