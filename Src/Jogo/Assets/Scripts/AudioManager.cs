
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float volume = 100f;
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume);
        }
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
    }
}
