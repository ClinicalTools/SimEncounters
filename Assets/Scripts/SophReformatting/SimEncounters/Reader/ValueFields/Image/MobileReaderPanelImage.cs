using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Image))]
    public class MobileReaderPanelImage : UIBehaviour
    {
        private float width;

        private const float Tolerance = .0001f;
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var currentWidth = ((RectTransform)transform).rect.width;
            if (Mathf.Abs(currentWidth - width) < Tolerance)
                return;

            width = currentWidth;
            UpdateHeight();
        }

        public string Name => name;
        public string Value { get; private set; } = null;

        public GameObject ImageGroup { get => imageGroup; set => imageGroup = value; }
        [SerializeField] private GameObject imageGroup;
        public float MaxHeight { get => maxHeight; set => maxHeight = value; }
        [SerializeField] private float maxHeight = -1;
        public LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }
        [SerializeField] private LayoutElement layoutElement;
        public Button EnlargeImageButton { get => enlargeImageButton; set => enlargeImageButton = value; }
        [SerializeField] private Button enlargeImageButton;

        private Image image;
        protected Image Image
        {
            get {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
        }

        protected SpriteDrawer SpritePopup { get; set; }
        protected ISelectedListener<Encounter> EncounterSelectedListener { get; set; }
        protected ISelectedListener<Panel> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            SpriteDrawer spritePopup,
            ISelectedListener<Encounter> encounterSelectedListener,
            ISelectedListener<Panel> panelSelectedListener)
        {
            SpritePopup = spritePopup;
            EncounterSelectedListener = encounterSelectedListener;
            PanelSelectedListener = panelSelectedListener;
        }

        protected override void Start()
        {
            base.Start();
            PanelSelectedListener.AddSelectedListener(OnPanelSelected);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            PanelSelectedListener.RemoveSelectedListener(OnPanelSelected);
        }

        protected virtual void OnPanelSelected(object sender, Panel panel)
        {
            if (!panel.Values.ContainsKey(Name)) {
                HideImage();
                return;
            }

            Value = panel.Values[Name];
            var sprites = EncounterSelectedListener.CurrentValue.Content.ImageContent.Sprites;
            if (Value != null && sprites.ContainsKey(Value))
                SetSprite(sprites[Value]);
            else
                HideImage();
        }

        protected virtual void HideImage()
        {
            if (ImageGroup != null)
                ImageGroup.SetActive(false);
        }

        protected Sprite Sprite { get; set; }
        protected virtual void SetSprite(Sprite sprite)
        {
            if (sprite == null) {
                Debug.LogError("Sprite is null");
                return;
            }

            Image.sprite = sprite;
            Sprite = sprite;

            UpdateHeight();
        }

        protected virtual void UpdateHeight()
        {
            if (Sprite == null)
                return;

            if (width == 0)
                width = ((RectTransform)transform).rect.width;

            var spriteHeight = Sprite.rect.height;
            var spriteWidth = Sprite.rect.width;
            var spriteRatio = spriteHeight / spriteWidth;

            if (EnlargeImageButton != null) {
                EnlargeImageButton.onClick.RemoveAllListeners();
                EnlargeImageButton.onClick.AddListener(() => SpritePopup.Display(Image.sprite));
            }

            if (LayoutElement == null)
                return;

            var height = width * spriteRatio;
            if (MaxHeight > Tolerance && MaxHeight < height) {
                height = MaxHeight;
                LayoutElement.preferredWidth = MaxHeight / spriteRatio;
                LayoutElement.flexibleWidth = -1;
            }
            LayoutElement.preferredHeight = height;
        }
    }
}