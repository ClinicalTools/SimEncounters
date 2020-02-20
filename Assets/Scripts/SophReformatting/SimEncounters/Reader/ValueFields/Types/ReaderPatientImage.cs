using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPatientImage : ReaderImage
    {

        private const string patientImageKey = "patientImage";
        public override void Initialize(ReaderScene reader)
        {
            Initialize(reader, patientImageKey);
        }
    }
}