using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimEncounters
{
    /// <remarks>
    /// This class act lke global data that can be referred to between scenes. 
    /// 
    /// This object is a monobehavior, so that if you want to override any part of it,
    /// you simply create a child class and include that class on an object instead of 
    /// this one in each scene.
    /// </remarks>
    public class SophGlobalData : MonoBehaviour
    {
        public static SophGlobalData Instance { get; protected set; }

        // TODO: store access to object containing user info
        public string FileName;

        private EncounterXml encounterXml;
        public virtual EncounterXml EncounterXml {
            get {
                if (encounterXml == null)
                    encounterXml = new EncounterXml();
                return encounterXml;
            }
            protected set {
                encounterXml = value;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
    }
}