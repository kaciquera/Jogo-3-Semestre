using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class DoorHover : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private GameObject buttons;

        private Animator _animator;
        private readonly int initiallizeHash = Animator.StringToHash("OnInitiallize");
        private readonly int activeHash = Animator.StringToHash("isActive");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            buttons.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OpenDoor();
        }

        private void OpenDoor()
        {
            _animator.SetTrigger(initiallizeHash);
            _animator.SetBool(activeHash, true);
        }

        public void ShowButtons()
        {
            buttons.SetActive(true);
        }
    }
}