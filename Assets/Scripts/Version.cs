using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

public static class Version {
	static List<int> _listVersion;

	public static int ClientVerMajor => ListVersion[0];
	public static int ClientVerMinor => ListVersion[1];
	public static int ClientVerRevision => ListVersion[2];

	public static int ClientVerQARevision {
		get => ListVersion[3];
		private set {
			ListVersion[3] = value;
#if UNITY_EDITOR
			PlayerSettings.bundleVersion = VersionConvertToString();
			_listVersion = null;
#endif
		}
	}

	static List<int> ListVersion {
		get {
			if ( _listVersion == null ) {
				var verArray = Application.version.Split('.');
				_listVersion = new List<int>();
				Array.ForEach(verArray, value => _listVersion.Add(int.Parse(value)));
			}

			return _listVersion;
		}
	}

	static string VersionConvertToString() {
		return string.Format("{0}.{1}.{2}.{3}", ClientVerMajor, ClientVerMinor, ClientVerRevision, ClientVerQARevision);
	}

	public static void UpdateQARevision() {
		ClientVerQARevision++;
		Debug.Log("New version: " + Application.version);
	}
}