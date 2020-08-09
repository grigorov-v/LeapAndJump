namespace Grigorov.Controllers {
    public interface IDeinit {
        ///<summary>Called when the "ControllerManager" is removed (when the game is closed)</summary>
        void OnDeinit();
    }
}