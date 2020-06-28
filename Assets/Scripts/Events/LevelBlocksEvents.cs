using Grigorov.LeapAndJump.Player;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.Level.Gameplay;

namespace Grigorov.LeapAndJump.Events {
    public struct PlayerIntoBlockTriggerEnter {
        public LevelBlock    LevelBlock {get; private set;}
        public PlayerControl Player     {get; private set;}

        public PlayerIntoBlockTriggerEnter (LevelBlock levelBlock, PlayerControl player) {
            LevelBlock = levelBlock;
            Player = player;
        }
    }

    public struct DestructionObjectPlayerCollision {
        public DestructionObject DestructionObject {get; private set;}

        public DestructionObjectPlayerCollision (DestructionObject destructionObject) {
            DestructionObject = destructionObject;
        }
    }
}