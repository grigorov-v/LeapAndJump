using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace Grigorov.LeapAndJump.Level
{
	public class Balance
	{
		public string World { get; private set; }

		JSONNode _jsonNode = null;

		public Balance(string world)
		{
			var jsonFromResources = Resources.Load($"Balance/{world}") as TextAsset;
			_jsonNode = JSON.Parse(jsonFromResources.text);
			World = world;
		}

		public List<string> GetElementsGroups(int level)
		{
			var arrayFromJson = _jsonNode["levels"][level.ToString()]["elements_groups"].AsArray;
			var result = new List<string>(arrayFromJson.Count);
			foreach (var pair in arrayFromJson)
			{
				result.Add(pair.Value);
			}

			return result;
		}

		public int GetFoodsCount(int level)
		{
			return _jsonNode["levels"][level.ToString()]["foods_count"].AsInt;
		}
	}
}
