using Guinea.Core;

namespace Guinea.Destructible
{
    public class DestructiblePlayer : Destructible
    {
        protected override void DestroyWhenDied()
        {
            MasterManager.GetLevelManager().CallGameOverLose();
            base.DestroyWhenDied();
        }
    }
}