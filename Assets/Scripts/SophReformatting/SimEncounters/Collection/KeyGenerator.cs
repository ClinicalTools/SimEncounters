using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicalTools.SimEncounters.Collections
{
    public class KeyGenerator
    {
        public static KeyGenerator Instance { get; protected set; } = new KeyGenerator(0);
        public static void ResetKeyGenerator(int seed) => Instance = new KeyGenerator(seed);


        protected int KeyByteLength { get; } = 3;
        protected int KeyCharLength => 3;

        protected virtual HashSet<string> Keys { get; } = new HashSet<string>();
        protected virtual System.Random KeyRandomizer { get; set; }

        public KeyGenerator(int seed)
        {
            KeyRandomizer = new System.Random(seed);
        }

        public virtual void SetSeed(int seed)
        {
            KeyRandomizer = new System.Random(seed);
        }

        public virtual bool Contains(string key) => Keys.Contains(key);
        public virtual void AddKey(string key)
        {
            if (!Keys.Contains(key))
                Keys.Add(key);
        }

        public virtual string Generate()
        {
            return Generate(KeyRandomizer);
        }

        public virtual string Generate(string seed)
        {
            var keyRandomizer = new Random(seed.GetHashCode());

            return Generate(keyRandomizer);
        }

        protected virtual string Generate(Random keyRandomizer)
        {
            var bytes = new byte[KeyByteLength];
            keyRandomizer.NextBytes(bytes);
            var key = Convert.ToBase64String(bytes);
            key = key.Substring(0, KeyCharLength);

            if (Keys.Contains(key))
                return Generate(keyRandomizer);

            Keys.Add(key);
            return key;
        }

        public bool IsValidKey(string key)
        {
            return key.Length == KeyCharLength;
        }
    }
}