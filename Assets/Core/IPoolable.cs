namespace Overwave
{
    public interface IPoolable
    {
        void OnSummon() { }
        void OnDelete() { }
        
        bool Deleted { get; set; }
    }
}