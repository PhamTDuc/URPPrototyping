namespace Guinea.Core
{
    public interface IManager
    {
        ManagerStatus status { get; }
        void Initialize();
    }
}