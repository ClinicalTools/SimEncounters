using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueEntryUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Image border;
        public Image Border { get => border; set => border = value; }

        [SerializeField] private List<GameObject> characterImages;
        public List<GameObject> CharacterImages { get => characterImages; set => characterImages = value; }

        public override void Display(UserPanel userPanel)
        {
            Debug.LogError("pleaseee implement");
        }
    }
}