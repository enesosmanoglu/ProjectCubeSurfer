using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton Bool
    public static SoundManager Instance { get; private set; }
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
        Debug.Log("### AWAKE ### SoundManager");
        volume = PlayerPrefs.GetFloat("effects_volume", volume);
        bgVolume = PlayerPrefs.GetFloat("music_volume", bgVolume);
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void Update()
    {
        if (backgroundMusic.Count > 0)
        {
            MainCamera.Instance.audioSource.volume = bgVolume * bgVolumeMultiplier;
        }
    }

    public void SetVolumeEffects(float value)
    {
        volume = value;
        PlayerPrefs.SetFloat("effects_volume", value);
    }

    public void SetVolumeMusic(float value)
    {
        bgVolume = value;
        PlayerPrefs.SetFloat("music_volume", value);
    }

    private float bgVolumeMultiplier = 0.3f;

    [Header("Settings")]
    [Range(0, 1)] public float volume = 0.6f;
    [Range(0, 1)] public float bgVolume = 0.2f;

    [Header("Sounds")]
    public List<AudioClip> backgroundMusic;
    public List<AudioClip> cubePickup;
    public List<AudioClip> cubeDrop;
    public List<AudioClip> diamondPickup;
    public List<AudioClip> levelPassed;
    public List<AudioClip> gameOver;
    public List<AudioClip> buttonClick;
    public List<AudioClip> lava;

    public void PlaySound(List<AudioClip> sounds, Transform target = null)
    {
        if (sounds.Count > 0)
        {
            int randomIndex = Random.Range(0, sounds.Count);
            if (target != null)
                AudioSource.PlayClipAtPoint(sounds[randomIndex], target.position, volume);
            else
                MainCamera.Instance.audioSource.PlayOneShot(sounds[randomIndex], volume * 2);
        }
    }

    public void PlayCubePickupSound(Transform target = null)
    {
        if (Managers.Game.gameState != GameState.Playing) return;
        PlaySound(cubePickup, target);
    }

    public void PlayDiamondPickupSound(Transform target = null)
    {
        if (Managers.Game.gameState == GameState.MainMenu) return;
        PlaySound(diamondPickup, target);
    }

    public void PlayCubeDropSound(Transform target = null)
    {
        if (Managers.Game.gameState != GameState.Playing) return;
        PlaySound(cubeDrop, target);
    }

    public void PlayLevelPassedSound(Transform target = null)
    {
        PlaySound(levelPassed, target);
    }

    public void PlayGameOverSound(Transform target = null)
    {
        if (Managers.Game.gameState == GameState.MainMenu) return;
        PlaySound(gameOver, target);
    }
    public void PlayButtonClickSound(Transform target = null)
    {
        PlaySound(buttonClick, target);
    }

    public void PlayLavaSound(Transform target = null)
    {
        if (Managers.Game.gameState == GameState.MainMenu) return;
        PlaySound(lava, target);
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic.Count > 0)
        {
            int randomIndex = Random.Range(0, backgroundMusic.Count);
            AudioSource audioSource = Managers.Game.Camera.GetComponent<AudioSource>();
            audioSource.clip = backgroundMusic[randomIndex];
            audioSource.volume = bgVolume * bgVolumeMultiplier;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        AudioSource audioSource = Managers.Game.Camera.GetComponent<AudioSource>();
        audioSource.Stop();
    }

}
