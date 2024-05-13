using UnityEngine;

namespace Game
{
    public class LevelAlbum : MonoBehaviour
    {
        [SerializeField] private int initialLevel = 0;
        [SerializeField] private int lastLevel = 10;
        [SerializeField] private PrefabInstantiator<LevelCard> prefabInstantiator;

        private void OnValidate()
        {
            name = $"AlbumBackground [{initialLevel}-{lastLevel}]";
        }

        private void Start()
        {
            for (int i = 0; i < lastLevel - initialLevel + 1; i++)
            {
                int levelIndex = initialLevel + i;
                prefabInstantiator.Instatiate(x => x.Initialize($"Sceve_Level_{levelIndex}", levelIndex));
            }
        }
    }

}
