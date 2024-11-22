using UnityEngine;
namespace Game
{
    public class DeleteSaveButtonBehavior : ButtonBehaviour
    {
        public override void OnClick()
        {
            PlayerPrefs.DeleteAll();

        }

    }
}
