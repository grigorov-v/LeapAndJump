public struct PlayerIntoBlockTriggerEnter {
    public LevelBlock     LevelBlock;
    public PlayerControl Player;

    public PlayerIntoBlockTriggerEnter (LevelBlock levelBlock, PlayerControl player) {
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