using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterCharactersPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;


        public WriterCharactersDrawer CharactersDrawer { get => charactersDrawer; set => charactersDrawer = value; }
        [SerializeField] private WriterCharactersDrawer charactersDrawer;
    }

    public class WriterCharactersDrawer : MonoBehaviour
    {

        public GameObject CharactersParent { get => charactersParent; set => charactersParent = value; }
        [SerializeField] private GameObject charactersParent;

        public Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;


        public WriterCharacter CharacterPrefab { get => characterPrefab; set => characterPrefab = value; }
        [SerializeField] private WriterCharacter characterPrefab;

    }

    public abstract class BaseWriterCharacter : MonoBehaviour
    {
    }

    public class WriterCharacter : BaseWriterCharacter
    {
        public BaseColorEditor ColorSelector { get => colorSelector; set => colorSelector = value; }
        [SerializeField] private BaseColorEditor colorSelector;
        public TMP_InputField RoleField { get => roleField; set => roleField = value; }
        [SerializeField] private TMP_InputField roleField;
        public TMP_Dropdown DialogueSideDropdown { get => dialogueSideDropdown; set => dialogueSideDropdown = value; }
        [SerializeField] private TMP_Dropdown dialogueSideDropdown;

        public BaseDragHandle DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private BaseDragHandle dragHandle;
        public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
        [SerializeField] private CanvasGroup canvasGroup;
        public Button DeleteButton { get => deleteButton; set => deleteButton = value; }
        [SerializeField] private Button deleteButton;



        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;




        protected virtual void ConfirmDelete() => ConfirmationPopup.ShowConfirmation(Delete, "Confirm", "Are you sure you want to remove this entry?");
        protected virtual void Delete()
        {
            //Deleted?.Invoke();
            Destroy(gameObject);
        }
    }
}