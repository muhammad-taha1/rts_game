using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<TKey, TValue>();

        //if (keys.Count != values.Count)
        //    throw new Exception($"Mismatched number of keys ({keys.Count}) and values ({values.Count})");

        for (int i = 0; i < keys.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }
    }

    public void Add(TKey key, TValue value)
    {
        dictionary[key] = value;
    }

    public bool Remove(TKey key)
    {
        return dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return new Dictionary<TKey, TValue>(dictionary);
    }

    public List<TKey> Keys => keys;
    public List<TValue> Values => values;
    public int Count => dictionary.Count;

    public TValue Get(TKey key)
    {
        return dictionary[key];
    }



}
