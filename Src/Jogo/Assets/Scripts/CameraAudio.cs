// 2024-05-29 AI-Tag 
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CameraAudio : MonoBehaviour
{
    public AudioClip clip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        MusicManager.Instance.ChangeMusic(clip);
    }
}
