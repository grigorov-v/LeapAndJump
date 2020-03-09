public struct PlayerIntoBlockTriggerEnter {
    public LevelBlock     LevelBlock;
    public PlayerMovement Player;

    public PlayerIntoBlockTriggerEnter (LevelBlock levelBlock, PlayerMovement player) {
        LevelBlock = levelBlock;
        Player = player;
    }
}
