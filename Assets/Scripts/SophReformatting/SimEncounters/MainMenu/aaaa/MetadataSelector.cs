
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public abstract class BaseMetadataSelector : MonoBehaviour
    {
        public abstract WaitableResult<KeyValuePair<SaveType, EncounterMetadata>> GetMetadata(MenuEncounter menuEncounter); 
    }

    public class MetadataSelector : BaseMetadataSelector
    { 
        public TextMeshProUGUI Label { get => label; set => label = value; }
        [SerializeField] private TextMeshProUGUI label;
        public Button LocalButton { get => localButton; set => localButton = value; }
        [SerializeField] private Button localButton;
        public Button ServerButton { get => serverButton; set => serverButton = value; }
        [SerializeField] private Button serverButton;
        public Button AutosaveButton { get => autosaveButton; set => autosaveButton = value; }
        [SerializeField] private Button autosaveButton;
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons;

        protected virtual void Awake()
        {
            LocalButton.onClick.AddListener(SelectLocal);
            ServerButton.onClick.AddListener(SelectServer);
            AutosaveButton.onClick.AddListener(SelectAutosave);
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Close);
        }

        protected virtual MenuEncounter CurrentEncounter { get; set; }
        protected virtual WaitableResult<KeyValuePair<SaveType, EncounterMetadata>> CurrentResult { get; set; }
        public override WaitableResult<KeyValuePair<SaveType, EncounterMetadata>> GetMetadata(MenuEncounter menuEncounter)
        {
            var metadatas = menuEncounter.Metadata;
            if (!metadatas.ContainsKey(SaveType.Local))
                return new WaitableResult<KeyValuePair<SaveType, EncounterMetadata>>(menuEncounter.GetLatestTypedMetada());

            var localMetadata = metadatas[SaveType.Local];
            var localModifiedTime = localMetadata.DateModified;

            var newerServer = IsMetadataNewer(metadatas, SaveType.Server, localModifiedTime);
            var newerAutosave = IsMetadataNewer(metadatas, SaveType.Autosave, localModifiedTime);
            if (!newerServer && !newerAutosave)
                return new WaitableResult<KeyValuePair<SaveType, EncounterMetadata>>(new KeyValuePair<SaveType, EncounterMetadata>(SaveType.Local, localMetadata));

            CurrentEncounter = menuEncounter;
            CurrentResult = new WaitableResult<KeyValuePair<SaveType, EncounterMetadata>>();

            gameObject.SetActive(true);
            serverButton.gameObject.SetActive(newerServer);
            autosaveButton.gameObject.SetActive(newerAutosave);
            Label.text = GetLabelText(menuEncounter, newerServer, newerAutosave);

            return CurrentResult;
        }

        protected virtual bool IsMetadataNewer(Dictionary<SaveType, EncounterMetadata> metadatas, SaveType saveType, long modifiedTime)
            => metadatas.ContainsKey(saveType) && metadatas[saveType].DateModified > modifiedTime;

        protected virtual void SelectLocal() => SelectMetadata(SaveType.Local);
        protected virtual void SelectServer() => SelectMetadata(SaveType.Server);
        protected virtual void SelectAutosave() => SelectMetadata(SaveType.Autosave);

        protected virtual void SelectMetadata(SaveType saveType)
        {
            if (CurrentResult?.IsCompleted() != false)
                throw new Exception("It shouldn't be possible to select the result again.");

            CurrentResult.SetResult(new KeyValuePair<SaveType, EncounterMetadata>(saveType, CurrentEncounter.Metadata[saveType]));
            gameObject.SetActive(false);
        }

        protected virtual string GetLabelText(MenuEncounter menuEncounter, bool newerServer, bool newerAutosave) {
            if (newerServer && newerAutosave)
                return "Server and autosave version are both newer than the local version.\nSelect which version to open:";
            else if (newerServer)
                return "Server version is newer than the local version.\nSelect which version to open:";
            else if (newerAutosave)
                return "Autosave version is newer than the local version.\nSelect which version to open:";
            else
                throw new Exception("Popup shouldn't be shown if server and autosave version are older.");
        }

        protected virtual void Close()
        {
            if (CurrentResult?.IsCompleted() == false)
                CurrentResult.SetError(new Exception("Metadata selector closed."));

            gameObject.SetActive(false);
        }
    }
}