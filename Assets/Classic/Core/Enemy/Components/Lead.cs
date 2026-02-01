using Overwave.Classic.Tower;

namespace Overwave.Classic.Enemy.Components
{
    [System.Serializable]
    public class Lead : Component
    {
        public bool isLead;
        
        public override ComponentType Type => ComponentType.Lead;

        public override bool RegisterDamageFromBullet(BulletController bullet)
            => bullet.Tower.Config.HasComponent(Tower.ComponentType.CanDamageLead);
    }
}