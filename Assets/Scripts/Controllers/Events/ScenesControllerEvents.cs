namespace Grigorov.LeapAndJump.Controllers.Events {
    public struct ScenesController_LoadedSceneEvent {
        public Scenes Scene { get; private set; }
        
        public ScenesController_LoadedSceneEvent(Scenes scene) {
            Scene = scene;
        }
    }
}