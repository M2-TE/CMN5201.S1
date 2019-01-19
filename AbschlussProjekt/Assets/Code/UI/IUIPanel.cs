public interface IUIPanel 
{
	void Register<T>(T manager) where T : IManager;
}
