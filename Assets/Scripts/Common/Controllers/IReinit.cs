namespace Grigorov.Controllers {
    public interface IReinit {
        ///<summary>Called when the "ControllerManager" is removed (when the game is closed)</summary>
        void OnReinit();
    }
}