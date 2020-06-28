using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace Grigorov.LeapAndJump.Level {
    public class LevelsBalance {
        JSONNode _jsonNode = null;

        public LevelsBalance(string world) {
            var jsonFromResources = Resources.Load($"Balance/{world}") as TextAsset;
            _jsonNode = JSON.Parse(jsonFromResources.text);
        }

        public List<string> GetElementsGroups(int level) {
            var arrayFromJson = _jsonNode["levels"][level.ToString()]["elements_groups"].AsArray;
            var result = new List<string>(arrayFromJson.Count);
            foreach ( var pair in arrayFromJson ) {
                result.Add(pair.Value);
            }

            return result;
        }
    }
}
