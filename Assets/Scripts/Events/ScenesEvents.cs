using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.Events {
    public struct LoadedScene {
        public Scenes Scene;
        
        public LoadedScene(Scenes scene) {
            Scene = scene;
        }
    }
}