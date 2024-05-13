using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LevelCard : LoadSceneButtonBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image lockImage;
        [SerializeField] private Sprite lockedIcon;
        [SerializeField] private Sprite unlockedIcon;

        public void Initialize(string levelName, int index)
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            sceneName = levelName;
            levelText.text = index.ToString("00");
            name = $"SelectableLevel [{index}]";
            bool isUnlocked = PlayerPrefs.HasKey("CurrentLevel") ? index <= PlayerPrefs.GetInt("CurrentLevel") + 1 : index == 0;
            lockImage.sprite = isUnlocked ? unlockedIcon : lockedIcon;
            button.interactable = isUnlocked;
        }
    }
}
