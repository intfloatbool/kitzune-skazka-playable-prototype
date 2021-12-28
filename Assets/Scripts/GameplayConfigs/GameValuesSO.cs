using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

namespace GameplayConfigs
{
    [CreateAssetMenu(fileName = "GameValuesSO", menuName = "Data/GameValuesSO")]
    public class GameValuesSO : ScriptableObject, IGameValuesProvider
    {
        [System.Serializable]
        private class CustomKVP<TValue>
        {
            [SerializeField] private string _nameKey;
            public string NameKey => _nameKey;

            [SerializeField] private TValue _value;
            public TValue Value => _value;

        }

        [SerializeField] private CustomKVP<int>[] _integerValues;

        private Dictionary<string, int> _integerValuesDict;

        [SerializeField] private CustomKVP<float>[] _floatValues;
        private Dictionary<string, float> _floatValuesDict;

        [SerializeField] private CustomKVP<bool>[] m_boolValues;
        private Dictionary<string, bool> m_boolValuesDict;


        [SerializeField] private CustomKVP<Color>[] m_colorValues;
        private Dictionary<string, Color> m_colorValuesDict;

        public int GetInt(string key)
        {
            if (_integerValuesDict == null)
            {
                _integerValuesDict = new Dictionary<string, int>();
                foreach (var i in _integerValues)
                {
                    _integerValuesDict.Add(i.NameKey, i.Value);
                }
            }

            if (_integerValuesDict.TryGetValue(key, out int result))
            {
                return result;
            }

            Debug.LogError($"{nameof(_integerValues)} has no key: {key}!");
            return 0;
        }

        public float GetFloat(string key)
        {
            if (_floatValuesDict == null)
            {
                _floatValuesDict = new Dictionary<string, float>();
                foreach (var i in _floatValues)
                {
                    _floatValuesDict.Add(i.NameKey, i.Value);
                }
            }

            if (_floatValuesDict.TryGetValue(key, out float result))
            {
                return result;
            }

            Debug.LogError($"{nameof(_floatValues)} has no key: {key}!");
            return 0;
        }

        public string GetString(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool GetBool(string _key)
        {
            if (m_boolValuesDict == null)
            {
                m_boolValuesDict = new Dictionary<string, bool>();
                foreach (var i in m_boolValues)
                {
                    m_boolValuesDict.Add(i.NameKey, i.Value);
                }
            }

            if (m_boolValuesDict.TryGetValue(_key, out bool result))
            {
                return result;
            }

            Debug.LogError($"{nameof(m_boolValues)} has no key: {_key}!");
            return false;
        }

        public Vector2 GetVector2(string key)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetVector3(string key)
        {
            throw new System.NotImplementedException();
        }

        public Color GetColor(string _key)
        {
            if (m_colorValuesDict == null)
            {
                m_colorValuesDict = new Dictionary<string, Color>();
                foreach (var i in m_colorValues)
                {
                    m_colorValuesDict.Add(i.NameKey, i.Value);
                }
            }

            if (m_colorValuesDict.TryGetValue(_key, out Color result))
            {
                return result;
            }

            Debug.LogError($"{nameof(m_colorValuesDict)} has no key: {_key}!");
            return Color.magenta;
        }
    }
}
