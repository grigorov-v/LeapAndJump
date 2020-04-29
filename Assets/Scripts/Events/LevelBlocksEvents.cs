using Game.Level;
using Game.Player;

namespace Game.Events {
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