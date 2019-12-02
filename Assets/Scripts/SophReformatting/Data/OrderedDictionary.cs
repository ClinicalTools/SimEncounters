using System.Collections;
using System.Collections.Generic;

namespace SimEncounters
{
    /// <summary>
    /// An ordered bidirectional dictionary with automatically generated string keys.
    /// </summary>
    /// <typeparam name="T">Type of values stored in the collection</typeparam>
    public class OrderedDictionary<T> : KeyedCollection<T>, IEnumerable<KeyValuePair<string, T>>
    {
        // An ordered list of values
        protected virtual List<T> ValueList { get; set; } = new List<T>();
        // An ordered list of the key value pairs
        // This could be made to use less memory, but both parts may be used frequently, so this is simpler
        protected virtual List<KeyValuePair<string, T>> PairList { get; set; } = new List<KeyValuePair<string, T>>();

        /// <summary>
        /// Ordered array of values in the collection.
        /// </summary>
        public virtual T[] ValueArr => ValueList.ToArray();
        public override IEnumerable<T> Values => ValueList;

        public virtual KeyValuePair<string, T> this[int index] => Get(index);

        public OrderedDictionary() : base() { }
        public OrderedDictionary(List<KeyValuePair<string, T>> keyValuePairs) : base(keyValuePairs) { }

        public override void Add(string key, T value)
        {
            var pair = new KeyValuePair<string, T>(key, value);
            Add(pair);
        }
        public override void Add(KeyValuePair<string, T> pair)
        {
            base.Add(pair);
            PairList.Add(pair);
            ValueList.Add(pair.Value);
        }

        public override void Remove(string key)
        {
            var index = ValueList.IndexOf(Collection[key]);
            ValueList.RemoveAt(index);
            PairList.RemoveAt(index);

            base.Remove(key);
        }

        public virtual KeyValuePair<string, T> Get(int val) => PairList[val];

        public virtual void MoveValue(int newIndex, int currentIndex) 
            => MoveValue(newIndex, ValueList[currentIndex]);
        public virtual void MoveValue(int newIndex, T value)
        {
            ValueList.Remove(value);
            ValueList.Insert(newIndex, value);
        }

        IEnumerator IEnumerable.GetEnumerator() => PairList.GetEnumerator();
        public override IEnumerator<KeyValuePair<string, T>> GetEnumerator() => PairList.GetEnumerator();

    }
}