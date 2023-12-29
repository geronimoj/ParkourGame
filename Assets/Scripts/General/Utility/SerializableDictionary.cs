using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> m_keys = null;

    [SerializeField]
    private List<TValue> m_values = null;

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        m_keys = new List<TKey>();
        m_values = new List<TValue>();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            m_keys.Add(pair.Key);
            m_values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        Clear();

        if (m_keys.Count != m_values.Count)
            throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.", m_keys.Count, m_values.Count));

        for (int i = 0; i < m_keys.Count; i++)
            Add(m_keys[i], m_values[i]);

        m_keys = null;
        m_values = null;
    }
}
