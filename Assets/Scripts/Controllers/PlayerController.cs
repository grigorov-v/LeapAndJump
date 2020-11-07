using UnityEngine;

using Grigorov.Controllers;
using Grigorov.Unity.Events;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers
{
	[ControllerAttribute]
	public class PlayerController : Controller
	{
		public override void OnAwake()
		{
			EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
		}

		public override void OnDestroy()
		{
			EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
		}

		public override void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Player.ForAll(player => player.JumpInput());
			}
		}

		public override void OnFixedUpdate()
		{
			Player.ForAll(player => player.OnUpdate());
		}

		void OnPointerDown(TapZone_PointerDown e)
		{
			Player.ForAll(player => player.JumpInput());
		}
	}
}