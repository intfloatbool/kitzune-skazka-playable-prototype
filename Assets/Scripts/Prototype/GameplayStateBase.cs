using System.Collections.Generic;

namespace Prototype
{
    public abstract class GameplayStateBase
    {
        protected Dictionary<string, string> _parameters;
        public virtual void SetParams(Dictionary<string, string> parameters)
        {
            _parameters = parameters;
        } 
        public abstract void OnStateStarted();
        public abstract void Update();
        public abstract void OnStateStopped();
    }
}
