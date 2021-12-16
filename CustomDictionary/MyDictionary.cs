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
        private int _count;
        public Dict()
        {
            _keys = new List<TKey>();
            _values = new List<TValue>();
            _items = new LinkedList<KeyValuePair<TKey, TValue>>[15];
        }
        public int Count => _count;

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
            _count++;
        }

        public bool ContainsKey(TKey key)
        {
            var hash = GetHashValue(key);
            return _items[hash] == null ? false : _items[hash].Any(p =>p.Key.Equals(key));
        }

        public TValue GetValue(TKey key)
        {
            var hash = GetHashValue(key);
            if (_items[hash] == null)
            {
                throw new Exception("NO FOUND KEY");
            }
            else 
            {
                return _items[hash].First(m => m.Key.Equals(key)).Value;
            }
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
            _keys = new List<TKey>();
            _values = new List<TValue>();
            _items = new LinkedList<KeyValuePair<TKey, TValue>>[15];
            _count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int hash = Math.Abs(item.Key.GetHashCode()) % _items.Length;
            if (_items[hash].Count != 0)
            {
                return true;
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int hash = Math.Abs(item.Key.GetHashCode()) % _items.Length;
            if(_items[hash] == null)
            {
                throw new Exception("Not found item in dictionary");
            }
            else
            {
                if (_items[hash].Count == 0)
                {
                    return false;
                }
                else
                {
                    return _items[hash].Remove(item);
                }
            }
        }

        public bool Remove(TKey key)
        {
            int hash = Math.Abs(key.GetHashCode()) % _items.Length;
            if (_items[hash] == null)
            {
                throw new Exception("Not found item in dictionary");
            }
            else
            {
                if (_items[hash].Count == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var item in _items[hash])
                    {
                        if (item.Key.Equals(key))
                        {
                            _items[hash].Remove(item);
                            _keys.Remove(key);
                            _values.Remove(item.Value);
                            _count--;
                            return true;
                        }
                    }
                }
            }
            return false;
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
                int hash = GetHashValue(key);
                if (_items[hash] == null) throw new KeyNotFoundException("Keys not found");
                if (_items[hash].Count != 0)
                {
                    foreach (var item in _items[hash])
                    {
                        if (item.Key.Equals(key))
                        {
                            return item.Value;
                        }
                    }
                }
                return (TValue)default;
            }
            set
            {
                int hash = GetHashValue(key);
                if (_items[hash].Count != 0)
                {
                    var temp = new KeyValuePair<TKey, TValue>();
                    foreach (var item in _items[hash])
                    {
                        if (item.Key.Equals(key))
                        {
                            temp = item;
                        }
                    }
                    _items[hash].Remove(temp);
                    _items[hash].AddLast(new KeyValuePair<TKey, TValue>(temp.Key, value));
                }
            }
        }
    }
}
