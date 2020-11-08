using UnityEngine;

using Grigorov.Controllers;
using Grigorov.Unity.Events;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers
{
	[Controller]
	public class PlayerController : ControllerForComponent<Player>, IAwake, IDestroy, IUpdate, IFixedUpdate
	{
		public void OnAwake()
		{
			EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
		}

		public void OnDestroy()
		{
			EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
		}

		public void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				CallAction(player => player.JumpInput());
			}
		}

		public void OnFixedUpdate()
		{
			CallAction(player => player.OnFixedUpdate());
		}

		void OnPointerDown(TapZone_PointerDown e)
		{
			CallAction(player => player.JumpInput());
		}
	}
}