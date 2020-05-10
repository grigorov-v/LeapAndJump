using UnityEditor;
using UnityEngine;

using System.IO;

namespace Grigorov.CustomBuildPipeline {
    public class BuildOptions {
        public static BuildPlayerOptions BuildPlayerOptions = new BuildPlayerOptions() {
            locationPathName = GetApkLocationPath("Builds", "LeapAndJump_" + Application.version),
            target = BuildTarget.Android,

            scenes = new string[] {
                "Assets/Scenes/Loading.unity",
                "Assets/Scenes/MainMenu.unity",
                "Assets/Scenes/World_1.unity"
            }
        };

        static string GetApkLocationPath(string directory, string nameApk) {
            if ( !nameApk.EndsWith(".apk") ) {
                nameApk += ".apk";
            }

            return Path.Combine(directory, nameApk);
        }
    }
}