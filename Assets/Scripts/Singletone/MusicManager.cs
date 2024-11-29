using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Background Music for Each Scene")]
    [SerializeField] private List<AudioClip> sceneMusic;

    private AudioSource audioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        UpdateAmbientSound(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateAmbientSound(scene.buildIndex);
    }

    private void UpdateAmbientSound(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= sceneMusic.Count)
        {
            Debug.LogWarning("No background music assigned for this scene.");
            return;
        }

        AudioClip newClip = sceneMusic[sceneIndex];

        if (newClip != null && audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}
