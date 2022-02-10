using UnityEngine;

namespace Prototype.Novel
{
    public abstract class NovelCommandBase 
    {
         public bool IsRunning { get; protected set; }

         private readonly VisualNovelController _novelController;
         
         public NovelCommandBase(VisualNovelController novelController)
         {
             this._novelController = novelController;
         }
         
         public virtual void Execute()
         {
             IsRunning = true;
             
             Debug.Log($"[Novel] Command with class {GetType().Name} Executed.");
         }

         public virtual void Stop()
         {
             Debug.Log($"[Novel] Command with class {GetType().Name} Stopped.");
         }

         public virtual void UpdateCommand()
         {
             
         }
    }
}
