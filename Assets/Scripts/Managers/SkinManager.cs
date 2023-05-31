using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    #region Singleton Bool
    public static SkinManager Instance { get; private set; }
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
        Debug.Log("### AWAKE ### SkinManager");
    }

    private void Start()
    {
        string skin = PlayerPrefs.GetString("selected_cube_skin", "0");
        if (skin.StartsWith("p"))
        {
            SetCubeSkinPriced(Int32.Parse(skin.Substring(1)));
        }
        else
        {
            SetCubeSkin(Int32.Parse(skin));
        }
    }

    public void SetCubeSkin(int index)
    {
        Debug.Log("[SKINMANAGER] Setting cube skin to " + index);
        currentCubeMaterial = cubeMaterials[index];
        PlayerPrefs.SetString("selected_cube_skin", index.ToString());
        SetAllCubeSkins();
    }

    public void SetCubeSkinPriced(int index)
    {
        Debug.Log("[SKINMANAGER] [PRICED] Setting cube skin to " + index);
        currentCubeMaterial = cubeMaterialsPriced[index];
        PlayerPrefs.SetString("selected_cube_skin", "p" + index.ToString());
        SetAllCubeSkins();
    }

    public GameObject SetCurrentCubeSkin(GameObject cube)
    {
        cube.GetComponentInChildren<MeshRenderer>().material = currentCubeMaterial;
        return cube;
    }

    public void SetAllCubeSkins()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject cube in cubes)
        {
            cube.GetComponentInChildren<MeshRenderer>().material = currentCubeMaterial;
        }
    }

    public Material currentCubeMaterial;
    public Material[] cubeMaterials;
    public Material[] cubeMaterialsPriced;
    public String[] cubePrices;

}
