using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.Controllers;
using Grigorov.Unity.Events;
using Grigorov.Unity.SceneManagement;
using Grigorov.Unity.SceneManagement.UI;

using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.Events;

namespace Grigorov.LeapAndJump.Controllers
{
	[ControllerAttribute]
	public class ScenesController : Controller
	{
		const string LoadingUIResource = "Prefabs/LoadingUI";
		const string WorldScenePrefix  = "World_";
		const string MainMenuScene     = "MainMenu";
		const string LoadingScene      = "Loading";

		LoadingUI _loadingUI = null;

		public bool   IsActiveWorldScene => CurrentSceneName.StartsWith(WorldScenePrefix);
		public string CurrentSceneName   => SceneManager.GetActiveScene().name;

		LoadingUI LoadingUI
		{
			get
			{
				if (!_loadingUI)
				{
					_loadingUI = FindOrCreateLoadingUI();
				}
				return _loadingUI;
			}
		}

		public override void OnAwake()
		{
			if (SceneManager.GetActiveScene().name == LoadingScene)
			{
				OpenMainMenu();
			}
		}

		public void OpenScene(string scene)
		{
			SceneLoadingHelper.StartLoadingScene(scene, LoadingUI)
				 .AddLoadedAction(_ => EventManager.Fire(new ScenesController_LoadedSceneEvent(scene)));
		}

		public void OpenLevel(LevelId level)
		{
			var world = level.World;
			OpenScene(world);
		}

		public void OpenMainMenu()
		{
			OpenScene(MainMenuScene);
		}

		public void RestartCurrentScene()
		{
			var curScene = SceneManager.GetActiveScene().name;
			OpenScene(curScene);
		}

		LoadingUI FindOrCreateLoadingUI()
		{
			var result = MonoBehaviour.FindObjectOfType<LoadingUI>();
			if (result)
			{
				return result;
			}

			result = Resources.Load<LoadingUI>(LoadingUIResource);
			return MonoBehaviour.Instantiate(result);
		}
	}
}