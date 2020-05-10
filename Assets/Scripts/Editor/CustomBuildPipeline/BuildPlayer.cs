using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

using System.IO;
using System.Linq;

namespace Grigorov.CustomBuildPipeline {
    public class BuildPlayer {
        [MenuItem("Build/Android Build")]
        public static void AndroidBuild() {
            Version.UpdateQARevision();
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(settingsScene => settingsScene.path).ToArray();
            buildPlayerOptions.locationPathName = GetApkLocationPath("Builds", "LeapAndJump_" + Application.version);
            buildPlayerOptions.target = BuildTarget.Android;
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if ( summary.result == BuildResult.Succeeded ) {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if ( summary.result == BuildResult.Failed ) {
                Debug.Log("Build failed");
            }
        }

        [MenuItem("Build/Clear Apk Build Directory")]
        static void ClearApkBuildDirectory() {
            var folderName = "Builds";
            var dir = new DirectoryInfo(folderName);
            foreach( var fi in dir.GetFiles() ) {
                if ( fi.Name.EndsWith(".apk") ) {
                    fi.Delete();
                }
            }
        }

        [MenuItem("Build/Update QA Revision")]
        static void UpdateQARevision() {
            Version.UpdateQARevision();
        }

        static string GetApkLocationPath(string directory, string nameApk) {
            if ( !nameApk.EndsWith(".apk") ) {
                nameApk += ".apk";
            }

            return Path.Combine(directory, nameApk);
        }
    }
}