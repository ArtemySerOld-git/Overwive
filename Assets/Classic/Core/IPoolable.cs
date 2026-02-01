namespace Overwave.Classic
{
    public interface IPoolable
    {
        virtual void OnSummon() { }
        virtual void OnDelete() { }
        
        bool Deleted { get; set; }
    }
}