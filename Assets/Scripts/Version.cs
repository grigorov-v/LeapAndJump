using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

public static class Version {
	static List<int> _listVersion;

	static int ClientVerMajor => ListVersion[0];
	static int ClientVerMinor => ListVersion[1];
	static int ClientVerRevision => ListVersion[2];

	static int ClientVerQARevision {
		get => ListVersion[3];
		set {
			ListVersion[3] = value;
#if UNITY_EDITOR
			PlayerSettings.bundleVersion = VersionConvertToString();
			_listVersion = null;
#endif
		}
	}

	static List<int> ListVersion {
		get {
			if ( _listVersion != null ) {
				return _listVersion;
			}

			var verArray = Application.version.Split('.');
			_listVersion = new List<int>();
			Array.ForEach(verArray, value => _listVersion.Add(int.Parse(value)));

			return _listVersion;
		}
	}

	static string VersionConvertToString() {
		return $"{ClientVerMajor}.{ClientVerMinor}.{ClientVerRevision}.{ClientVerQARevision}";
	}

	public static void UpdateQaRevision() {
		ClientVerQARevision++;
		Debug.Log("New version: " + Application.version);
	}
}