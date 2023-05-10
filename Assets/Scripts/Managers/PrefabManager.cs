using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    #region Singleton
    public static PrefabManager Instance { get; private set; }
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

    [Header("Prefabs")]
    public GameObject cubePrefab;
    public GameObject playerPrefab;
    public GameObject diamondPrefab;
    public GameObject finishPrefab;
    public GameObject wallPrefab;
    public GameObject lavaPrefab;
    public GameObject platformPrefab;
    public GameObject wallCubeRotatingPrefab;
}
