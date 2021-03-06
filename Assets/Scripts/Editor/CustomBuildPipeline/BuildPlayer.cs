﻿using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Grigorov.CustomBuildPipeline {
	public class BuildPlayer {
		[MenuItem("Build/Android Build")]
		public static void AndroidBuild() {
			Version.UpdateQaRevision();

			var report = BuildPipeline.BuildPlayer(BuildOptions.BuildPlayerOptions);
			var summary = report.summary;
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
			foreach ( var fi in dir.GetFiles() ) {
				if ( fi.Name.EndsWith(".apk") ) {
					fi.Delete();
				}
			}
		}

		[MenuItem("Build/Update QA Revision")]
		static void UpdateQARevision() {
			Version.UpdateQaRevision();
		}
	}
}