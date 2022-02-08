namespace Prototype
{
    public abstract class GameplayStateBase
    {
        public abstract void OnStateStarted();
        public abstract void Update();
        public abstract void OnStateStopped();
    }
}
