namespace Overwave.Classic.Tower
{
    [System.Flags]
    public enum ClassType
    {
        None = 0,
        Starter = 1,
        Killer = 2,
        Tank = 4,
        Control = 8,
        Support = 16,
        Economy = 32,
        Damage = 64,
        Shooter = 128,
        Thrower = 256,
        Assistant = 512,
    }
}