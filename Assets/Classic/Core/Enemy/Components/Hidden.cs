namespace Overwave.Classic.Enemy.Components
{
    [System.Serializable]
    public class Hidden : Component
    {
        public bool hidden;
        
        public override ComponentType Type => ComponentType.Hidden;

        public override bool CanAddToTower(Tower.Behavior tower)
            => tower.Config.HasComponent(Tower.ComponentType.SeeHidden);
    }
}