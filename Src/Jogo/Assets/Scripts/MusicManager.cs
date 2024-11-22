// 2024-05-29 AI-Tag 
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public Slider volumeSlider;
    private AudioSource audioSource;
    private AudioClip nextClip;
    private float storedVolumeValue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void RegisterSlider(Slider slider)
    {
        volumeSlider = slider;
        volumeSlider.onValueChanged.AddListener(delegate { AdjustVolume(); });
        AdjustVolume();
    }

    private void OnEnable()
    {
        AdjustVolume();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject sliderMusic = GameObject.Find("SliderMusic");
        if (sliderMusic != null)
        {
            volumeSlider = sliderMusic.GetComponent<Slider>();
            volumeSlider.onValueChanged.AddListener(delegate { AdjustVolume(); });

            // Recupera o valor do volume salvo
            if (PlayerPrefs.HasKey("Volume"))
            {
                volumeSlider.value = PlayerPrefs.GetFloat("Volume");
            }
            AdjustVolume();
        }

    }

    public void AdjustVolume()
    {
        if (volumeSlider != null)
        {
            audioSource.volume = volumeSlider.value;
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        }
    }

    public void ChangeMusic(AudioClip clip)
    {
        if (clip != null)
        {
            storedVolumeValue = audioSource.volume;
            nextClip = clip;
            StartCoroutine(FadeOutAndIn());
        }
        else
        {
            Debug.LogError("Clip is null!");
        }
    }

    private IEnumerator FadeOutAndIn()
    {
        // Fade out
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= Time.deltaTime * storedVolumeValue;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        // Change clip and fade in
        audioSource.clip = nextClip;
        audioSource.Play();
        while (audioSource.volume < storedVolumeValue)
        {
            audioSource.volume += Time.deltaTime * storedVolumeValue;
            yield return null;
        }

        audioSource.volume = storedVolumeValue;
    }
}
