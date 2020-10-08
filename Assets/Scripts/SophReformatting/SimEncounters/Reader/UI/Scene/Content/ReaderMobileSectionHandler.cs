using ClinicalTools.SimEncounters.Collections;
using System.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionHandler : ReaderCompletableEncounterHandler
    {
        public MonoBehaviour MainSectionContentPrefab { get => mainSectionContentPrefab; set => mainSectionContentPrefab = value; }
        [SerializeField] private MonoBehaviour mainSectionContentPrefab;
        public MonoBehaviour MainSectionContentDefault { get => mainSectionContentDefault; set => mainSectionContentDefault = value; }
        [SerializeField] private MonoBehaviour mainSectionContentDefault;


        protected override void Awake()
        {
            CurrentSectionContent = MainSectionContentDefault;
            AddReaderObject(CurrentSectionContent);
            base.Awake();
        }

        protected MonoBehaviour CurrentSectionContent { get; set; }
        protected RectTransform CurrentSectionRectTransform => (RectTransform)CurrentSectionContent.transform;
        protected MonoBehaviour NewSectionContent { get; set; }
        protected RectTransform NewSectionContentRectTransform => (RectTransform)NewSectionContent.transform;

        protected virtual OrderedCollection<Section> Sections { get; set; }
        protected virtual UserSection CurrentSection { get; set; }
        public override void Display(UserEncounter userEncounter)
        {
            Sections = userEncounter.Data.Content.NonImageContent.Sections;
            base.Display(userEncounter);
        }

        public override void Display(UserSection userSection)
        {
            if (CurrentSection == null)
                CurrentSection = userSection;
            else if (CurrentSection != userSection)
                ChangeSections(userSection);

            base.Display(userSection);
        }

        protected virtual void ChangeSections(UserSection userSection)
        {
            Deregister(CurrentSectionContent);
            NewSectionContent = Instantiate(MainSectionContentPrefab, transform);
            NewSectionContentRectTransform.SetSiblingIndex(0);

            AddReaderObject(NewSectionContent);

            if (Sections.IndexOf(userSection.Data) > Sections.IndexOf(CurrentSection.Data))
                StartCoroutine(ShiftSectionForward());
            else
                StartCoroutine(ShiftSectionBackward());

            CurrentSection = userSection;
        }

        private const float MoveTime = .5f;
        protected virtual IEnumerator ShiftSectionForward()
        {
            float moveAmount = 0;

            while (moveAmount < 1) {
                moveAmount += Time.deltaTime / MoveTime;
                SetMoveAmountForward(moveAmount);
                yield return null;
            }

            Destroy(CurrentSectionContent.gameObject);
            CurrentSectionContent = NewSectionContent;
        }

        protected virtual void SetMoveAmountForward(float moveAmount)
        {
            moveAmount = Mathf.Clamp01(moveAmount);
            moveAmount = Curve(moveAmount);

            CurrentSectionRectTransform.anchorMin = new Vector2(0 - moveAmount, CurrentSectionRectTransform.anchorMin.y);
            CurrentSectionRectTransform.anchorMax = new Vector2(1 - moveAmount, CurrentSectionRectTransform.anchorMax.y);
            NewSectionContentRectTransform.anchorMin = new Vector2(1 - moveAmount, CurrentSectionRectTransform.anchorMin.y);
            NewSectionContentRectTransform.anchorMax = new Vector2(2 - moveAmount, CurrentSectionRectTransform.anchorMax.y);
        }

        protected virtual IEnumerator ShiftSectionBackward()
        {
            float moveAmount = 0;

            while (moveAmount < 1) {
                moveAmount += Time.deltaTime / MoveTime;
                SetMoveAmountBackward(moveAmount);
                yield return null;
            }

            Destroy(CurrentSectionContent.gameObject);
            CurrentSectionContent = NewSectionContent;
        }

        protected virtual void SetMoveAmountBackward(float moveAmount)
        {
            moveAmount = Mathf.Clamp01(moveAmount);
            moveAmount = Curve(moveAmount);

            CurrentSectionRectTransform.anchorMin = new Vector2(0 + moveAmount, CurrentSectionRectTransform.anchorMin.y);
            CurrentSectionRectTransform.anchorMax = new Vector2(1 + moveAmount, CurrentSectionRectTransform.anchorMax.y);
            NewSectionContentRectTransform.anchorMin = new Vector2(-1 + moveAmount, CurrentSectionRectTransform.anchorMin.y);
            NewSectionContentRectTransform.anchorMax = new Vector2(0 + moveAmount, CurrentSectionRectTransform.anchorMax.y);
        }

        protected virtual float Curve(float value) => Mathf.Clamp01(1 - (Mathf.Pow(2 * (1 - value), 2) / 4));
    }
}
