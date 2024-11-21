using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class DoorHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Animator _animator;
        private readonly int initiallizeHash = Animator.StringToHash("OnInitiallize");
        private readonly int activeHash = Animator.StringToHash("isActive");

        [SerializeField] private GameObject buttons; // Arraste o contêiner dos botões aqui no Inspector.
        private bool isMouseOverDoor = false;
        private bool isMouseOverButtons = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (buttons != null)
            {
                buttons.SetActive(false); 
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOverDoor = true;
            OpenDoor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOverDoor = false;
            CheckAndCloseDoor();
        }

        private void OpenDoor()
        {
            _animator.SetTrigger(initiallizeHash);
            _animator.SetBool(activeHash, true);

           
            if (buttons != null)
            {
                float animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
                Invoke(nameof(ShowButtons), animationLength); 
            }
        }

        private void ShowButtons()
        {
            if (isMouseOverDoor) 
            {
                buttons.SetActive(true);
            }
        }

        private void CheckAndCloseDoor()
        {
            
            if (!isMouseOverDoor && !isMouseOverButtons)
            {
                _animator.SetBool(activeHash, false);
                if (buttons != null)
                {
                    buttons.SetActive(false);
                }
            }
        }

        public void OnMouseEnterButtons()
        {
            isMouseOverButtons = true;
        }

        public void OnMouseExitButtons()
        {
            isMouseOverButtons = false;
            CheckAndCloseDoor();
        }
    }
}