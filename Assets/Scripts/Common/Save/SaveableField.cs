using UnityEngine;

namespace Grigorov.Save {
    public class SaveableField<T> {
        [System.Serializable]
        struct ValueContainer {
            public T Value;
        }

        string         _key            = string.Empty;
        ValueContainer _valueContainer = new ValueContainer();

        public T Value {
            get => _valueContainer.Value;
            set => _valueContainer.Value = value;
        }

        public SaveableField<T> SetKey(string key) {
            _key = key;
            return this;
        }

        public void Load(T defaultValue = default) {
            var json = PlayerPrefs.GetString(_key, null);
            if ( string.IsNullOrEmpty(json) ) {
                _valueContainer = new ValueContainer();
                _valueContainer.Value = defaultValue;
                return;
            }
            
            _valueContainer = JsonUtility.FromJson<ValueContainer>(json);
        }

        public void Save() {
            var json = JsonUtility.ToJson(_valueContainer);
            PlayerPrefs.SetString(_key, json);
        }
    }
}