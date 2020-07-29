using UnityEngine;

using Grigorov.Controllers;

namespace Grigorov.LeapAndJump.Controllers {
    public class TEstController : IInit, IAwake, IReinit, IUpdate {
        void IInit.OnInit() {
            Debug.Log("TEstController OnInit");
        }

        void IAwake.OnAwake() {
            Debug.Log("TEstController OnAwake");
        }

        void IReinit.OnReinit() {
            Debug.Log("TEstController OnReinit");
        }

        void IUpdate.OnUpdate() {
            //Debug.Log("TEstController OnUpdate");
        }
    }
}