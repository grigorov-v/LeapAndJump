using UnityEngine;

namespace Grigorov.Save {
	public class SaveableField<T> {
		readonly bool   _autoSave;
		readonly T      _defaultValue;
		readonly string _key;
		
		bool _isLoaded;
		
		ValueContainer<T> _valueContainer;

		public SaveableField(string key, bool autoSaveOnChange = false, T defaultValue = default) {
			_key = key;
			_autoSave = autoSaveOnChange;
			_defaultValue = defaultValue;
		}

		public T Value {
			get {
				if ( !_isLoaded ) {
					Load();
				}

				return _valueContainer.Value;
			}
			set {
				_valueContainer.Value = value;
				if ( _autoSave ) {
					Save();
				}
			}
		}

		public void Load() {
			var json = PlayerPrefs.GetString(_key, null);
			if ( string.IsNullOrEmpty(json) ) {
				_valueContainer.Value = _defaultValue;
				return;
			}

			_valueContainer = JsonUtility.FromJson<ValueContainer<T>>(json);
			_isLoaded = true;
		}

		public void Save() {
			var json = JsonUtility.ToJson(_valueContainer);
			PlayerPrefs.SetString(_key, json);
		}
	}
}