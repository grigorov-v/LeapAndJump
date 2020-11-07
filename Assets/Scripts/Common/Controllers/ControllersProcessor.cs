using UnityEngine;
using System.Collections.Generic;

namespace Grigorov.Controllers
{
	public class ControllersProcessor : MonoBehaviour
	{
		public static ControllersProcessor Instance { get; private set; }

		List<Controller> AllControllers => Controller.AllControllers;

		void Awake()
		{
			AllControllers.ForEach(controller => controller?.OnAwake());
			Instance = this;
		}

		void Start()
		{
			AllControllers.ForEach(controller => controller?.OnStart());
		}

		void Update()
		{
			AllControllers.ForEach(controller => controller?.OnUpdate());
		}

		void FixedUpdate()
		{
			AllControllers.ForEach(controller => controller?.OnFixedUpdate());
		}

		void OnDestroy()
		{
			AllControllers.ForEach(controller => controller?.OnDestroy());
		}
	}
}