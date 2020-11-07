using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Grigorov.Controllers
{
	public abstract class Controller
	{
		static List<Controller> _allControllers = new List<Controller>();

		public static List<Controller> AllControllers
		{
			get
			{
				if (_allControllers.Count == 0)
				{
					_allControllers = CreateAllControllers();
				}
				return _allControllers;
			}
		}

		public virtual void OnAwake() { }

		public virtual void OnStart() { }

		public virtual void OnUpdate() { }

		public virtual void OnFixedUpdate() { }

		public virtual void OnDestroy() { }

		public static T Get<T>() where T : Controller
		{
			var controller = AllControllers.Find(c => c is T);
			return (controller != null) ? controller as T : null;
		}

		static List<Controller> CreateAllControllers()
		{
			var outType = typeof(Controller);
			return Assembly.GetAssembly(outType).GetTypes()
					.Where(type => type.IsSubclassOf(outType))
					.Select(type => Activator.CreateInstance(type) as Controller)
					.ToList();
		}
	}
}