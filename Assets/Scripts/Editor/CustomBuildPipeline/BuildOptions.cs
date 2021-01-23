using System.IO;
using System.Linq;
using UnityEditor;

namespace Grigorov.CustomBuildPipeline {
	public static class BuildOptions {
		public static BuildPlayerOptions BuildPlayerOptions = new BuildPlayerOptions {
			locationPathName = GetApkLocationPath("Builds", "LeapAndJump_QA"),
			target = BuildTarget.Android,
			scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray()
		};

		static string GetApkLocationPath(string directory, string nameApk) {
			if ( !nameApk.EndsWith(".apk") ) {
				nameApk += ".apk";
			}

			return Path.Combine(directory, nameApk);
		}
	}
}