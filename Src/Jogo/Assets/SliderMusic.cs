// 2024-05-29 AI-Tag 
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;
using UnityEngine.UI;

public class SliderMusic : MonoBehaviour
{
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        MusicManager.Instance.RegisterSlider(slider);
    }
}
