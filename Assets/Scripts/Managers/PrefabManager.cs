using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    #region Singleton Bool
    public static PrefabManager Instance { get; private set; }
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
        Debug.Log("### AWAKE ### PrefabManager");
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
