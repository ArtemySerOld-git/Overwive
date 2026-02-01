namespace Overwave.Classic.Enemy.Components
{
    [System.Serializable]
    public class Fly : Component
    {
        public bool fly;
        
        public override ComponentType Type => ComponentType.Fly;

        public override bool CanAddToTower(Tower.Behavior tower)
            => tower.Config.HasComponent(Tower.ComponentType.SeeFly);
    }
}