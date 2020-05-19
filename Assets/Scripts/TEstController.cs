using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grigorov.Controller;

namespace Grigorov.LeapAndJump.Controllers {
    public class TEstController : BaseController<TEstController> {
        public override void Init() {
            base.Init();
            Debug.LogFormat("{0} Init", typeof(TEstController).ToString());
        }

        public override void PostInit() {
            Debug.LogFormat("{0} PostInit", typeof(TEstController).ToString());
        }

        public override void Reinit() {
            Debug.LogFormat("{0} Reinit", typeof(TEstController).ToString());
        }
    }
}