using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace CookingCapital.Common.UI.Editor {
	[CustomEditor(typeof(RegionContainer))]
	public sealed class RegionContainerEditor : UnityEditor.Editor {
		RegionContainer _targetRegionContainer;

		public RegionContainer TargetRegionContainer {
			get {
				if ( !_targetRegionContainer ) {
					_targetRegionContainer = (RegionContainer)target;
				}
				return _targetRegionContainer;
			}
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			foreach ( var itemsCity in TargetRegionContainer.Items.Cities ) {
				EditorGUILayout.LabelField(itemsCity.CityName);
				GUILayout.BeginHorizontal();

				if ( itemsCity.Items == null ) {
					continue;
				}

				foreach ( var region in itemsCity.Items ) {
					PauseWindowContainer pauseWindowContainer = region.GetComponent<PauseWindowContainer>();

					if ( pauseWindowContainer ) {
						Undo.RecordObject(pauseWindowContainer.BackgroundLeft, "Sprite Changed");
						Undo.RecordObject(pauseWindowContainer.BackgroundRight, "Sprite Changed");

						pauseWindowContainer.BackgroundLeft.sprite = (Sprite)EditorGUILayout.ObjectField(
							pauseWindowContainer.BackgroundLeft.sprite,
							typeof(Sprite),
							false, GUILayout.Width(100), GUILayout.Height(100));

						pauseWindowContainer.BackgroundRight.sprite = (Sprite)EditorGUILayout.ObjectField(
							pauseWindowContainer.BackgroundRight.sprite,
							typeof(Sprite),
							false, GUILayout.Width(100), GUILayout.Height(100));
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
					}
					Image regionImage = region.GetComponent<Image>();

					if ( regionImage ) {
						Undo.RecordObject(regionImage, "Sprite Changed");

						regionImage.sprite = (Sprite)EditorGUILayout.ObjectField(regionImage.sprite,
							typeof(Sprite),
							false, GUILayout.Width(75), GUILayout.Height(75));
					}
				}

				GUILayout.EndHorizontal();
			}
		}
	}
}