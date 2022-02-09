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
         }

         public virtual void Stop()
         {
             IsRunning = false;
         }

         public virtual void UpdateCommand()
         {
             
         }
    }
}
