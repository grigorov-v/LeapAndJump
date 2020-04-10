public struct PlayerIntoBlockTriggerEnter {
    public LevelBlock     LevelBlock;
    public PlayerMovement Player;

    public PlayerIntoBlockTriggerEnter (LevelBlock levelBlock, PlayerMovement player) {
        LevelBlock = levelBlock;
        Player = player;
    }
}

public struct DestructionObjectPlayerCollision {
    public DestructionObject DestructionObject;

    public DestructionObjectPlayerCollision (DestructionObject destructionObject) {
        DestructionObject = destructionObject;
    }
}