using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPatientImage : ReaderImage
    {

        private const string patientImageKey = "patientImage";
        public override void Initialize(UserPanel userPanel)
        {
            Initialize(userPanel, patientImageKey);
        }
    }
}