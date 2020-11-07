using UnityEngine;

namespace Grigorov.Save
{
	public class SaveableField<T>
	{
		string _key          = string.Empty;
		bool   _autosave     = false;
		bool   _isLoaded     = false;
		T      _defaultValue = default;

		ValueContainer<T> _valueContainer = new ValueContainer<T>();

		public T Value
		{
			get
			{
				if (!_isLoaded)
				{
					Load();
				}
				return _valueContainer.Value;
			}
			set
			{
				_valueContainer.Value = value;
				if (_autosave)
				{
					Save();
				}
			}
		}

		public SaveableField(string key, bool autosaveOnChange = false, T defaultValue = default)
		{
			_key = key;
			_autosave = autosaveOnChange;
			_defaultValue = defaultValue;
		}

		public void Load()
		{
			var json = PlayerPrefs.GetString(_key, null);
			if (string.IsNullOrEmpty(json))
			{
				_valueContainer.Value = _defaultValue;
				return;
			}

			_valueContainer = JsonUtility.FromJson<ValueContainer<T>>(json);
			_isLoaded = true;
		}

		public void Save()
		{
			var json = JsonUtility.ToJson(_valueContainer);
			PlayerPrefs.SetString(_key, json);
		}
	}
}