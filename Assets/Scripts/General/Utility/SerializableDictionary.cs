using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = null;

    [SerializeField]
    private List<TValue> values = null;

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys = new List<TKey>();
        values = new List<TValue>();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        Clear();

        if (keys.Count != values.Count)
            throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.", keys.Count, values.Count));

        for (int i = 0; i < keys.Count; i++)
            Add(keys[i], values[i]);

        keys = null;
        values = null;
    }
}
