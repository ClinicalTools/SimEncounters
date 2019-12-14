using System.Collections.Generic;
using System.Linq;

namespace ClinicalTools.SimEncounters.Collections
{
    public class KeyGenerator
    {
        public static KeyGenerator Instance { get; protected set; } = new KeyGenerator(0);
        public static void ResetKeyGenerator(int seed) => Instance = new KeyGenerator(seed);


        protected int KeyByteLength { get; } = 5;
        protected int KeyCharLength => KeyByteLength * 2;

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
            var keyRandomizer = new System.Random(seed.GetHashCode());

            return Generate(keyRandomizer);
        }

        protected virtual string Generate(System.Random keyRandomizer)
        {
            var bytes = new byte[5];
            var encoding = new System.Text.UTF8Encoding();
            keyRandomizer.NextBytes(bytes);
            // https://stackoverflow.com/a/623134
            var key = string.Concat(bytes.Select(b => b.ToString("x2")).ToArray());

            if (Keys.Contains(key))
                return Generate(keyRandomizer);


            return key;
        }
    }
}