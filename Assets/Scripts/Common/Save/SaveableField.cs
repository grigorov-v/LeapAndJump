using UnityEngine;

namespace Grigorov.Save {
    public class SaveableField<T> {
        [System.Serializable]
        struct ValueContainer {
            public T Value;
        }

        string         _key            = string.Empty;
        bool           _autosave       = false;
        bool           _isLoaded       = false;
        T              _defaultValue   = default;
        ValueContainer _valueContainer = new ValueContainer();

        public T Value {
            get {
                if ( !_isLoaded ) {
                    Load();
                }
                return _valueContainer.Value;
            } 
            set {
                _valueContainer.Value = value;
                if ( _autosave ) {
                    Save();
                }
            }
        }

        public SaveableField(string key, bool autosave = false, T defaultValue = default) {
            _key = key;
            _autosave = autosave;
            _defaultValue = defaultValue;
        }

        public void Load() {
            var json = PlayerPrefs.GetString(_key, null);
            if ( string.IsNullOrEmpty(json) ) {
                _valueContainer = new ValueContainer();
                _valueContainer.Value = _defaultValue;
                return;
            }
            
            _valueContainer = JsonUtility.FromJson<ValueContainer>(json);
            _isLoaded = true;
        }

        public void Save() {
            var json = JsonUtility.ToJson(_valueContainer);
            PlayerPrefs.SetString(_key, json);
        }
    }
}