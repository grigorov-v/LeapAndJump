using UnityEngine;
using System.IO;

using SimpleJSON;

public static class Version {
    static JSONNode _jsonNode            = null;

    public static int ClientVerMajor {
        get {
            return JSONNode["clientVerMajor"].AsInt;
        }
    }

    public static int ClientVerMinor{
        get {
            return JSONNode["clientVerMinor"].AsInt;
        }
    }

    public static int ClientVerRevision {
        get {
            return JSONNode["clientVerRevision"].AsInt;
        }
    }

    public static int ClientVerQARevision {
        get {
            return JSONNode["clientVerQARevision"].AsInt;
        }
    }

    public static string GetStringVersion() {
        return string.Format("{0}.{1}.{2}.{3}", Version.ClientVerMajor, Version.ClientVerMinor, Version.ClientVerRevision, Version.ClientVerQARevision);
    }

    static JSONNode JSONNode {
        get {
            if ( _jsonNode == null ) {
                var versionText = Resources.Load<TextAsset>("Version").text;
                _jsonNode = JSON.Parse(versionText);
            }

            return _jsonNode;
        }
    }

    public static void UpdateQARevision() {
        var writePath = "Assets/Resources/Version.json";
        JSONNode["clientVerQARevision"].AsInt ++;
        using (StreamWriter sw = new StreamWriter(writePath)) {
            sw.WriteLine(JSONNode.ToString());
        }

        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(writePath);
        #endif
    }
}