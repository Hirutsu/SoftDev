using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDictionary
{
    public class Dict<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private List<TKey> _keys;
        private List<TValue> _values;
        private LinkedList<KeyValuePair<TKey, TValue>>[] _items;
        private int _capacity;
        public Dict()
        {
            _keys = new List<TKey>();
            _values = new List<TValue>();
            _items = new LinkedList<KeyValuePair<TKey, TValue>>[15];
        }
        public int Count => _capacity;

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => _keys;

        public ICollection<TValue> Values => _values;

        public void Add(TKey key, TValue val)
        {
            var hash = GetHashValue(key);
            if (_items[hash] == null)
            {
                _items[hash] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }
            var keyPresent = _items[hash].Any(p => p.Key.Equals(key));
            if (keyPresent)
            {
                throw new Exception("An item with the same key has already been added.");
            }
            var newValue = new KeyValuePair<TKey, TValue>(key, val);
            _items[hash].AddLast(newValue);
            _keys.Add(key);
            _values.Add(val);
            _capacity++;
        }

        public bool ContainsKey(TKey key)
        {
            var hash = GetHashValue(key);
            return _items[hash] == null ? false : _items[hash].Any(p =>p.Key.Equals(key));
        }

        public TValue GetValue(TKey key)
        {
            var hash = GetHashValue(key);
            return _items[hash] == null ? default(TValue) :
                _items[hash].First(m => m.Key.Equals(key)).Value;
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from collections in _items
                    where collections != null
                    from item in collections
                    select item).GetEnumerator();
        }

        private int GetHashValue(TKey key)
        {
            return (Math.Abs(key.GetHashCode())) % _items.Length;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _items = new LinkedList<KeyValuePair<TKey, TValue>>[15];
            _capacity = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(TKey key)
        {
            int hash = Math.Abs(key.GetHashCode()) % _items.Length;
            if (_items[hash].Count == 0)
            {
                return false;
            }
            else
            {
                KeyValuePair<TKey, TValue> temp = default;
                foreach (var item in _items[hash])
                {
                    if (item.Key.Equals(key))
                    {
                        _keys.Remove(key);
                        _values.Remove(item.Value);
                        temp = item;
                    }
                }
                if (temp.Equals((KeyValuePair<TKey, TValue>)default))
                {
                    return false;
                }
                else
                {
                    _items[hash].Remove(temp);
                }
                return true;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int hash = Math.Abs(key.GetHashCode()) % _items.Length;
            if (_items[hash].Count == 0)
            {
                value = default;
                return false;
            }
            else
            {
                foreach (var item in _items[hash])
                {
                    if (item.Key.Equals(key))
                    {
                        value = item.Value;
                        return true;
                    }
                }
            }
            value = (TValue)default;
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                int h = GetHashValue(key);
                if (_items[h] == null) throw new KeyNotFoundException("Keys not found");
                return _items[h].FirstOrDefault(p => p.Key.Equals(key)).Value;
            }
            set
            {
                int h = GetHashValue(key);
                _items[h] = new LinkedList<KeyValuePair<TKey, TValue>>();
                _items[h].AddLast(new KeyValuePair<TKey, TValue> (key, value));
            }
        }
    }
}
