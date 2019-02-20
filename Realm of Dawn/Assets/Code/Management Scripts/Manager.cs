public abstract class Manager
{
	public Manager()
	{
		AssetManager instance = AssetManager.Instance;
		if (!instance.ActiveManagers.Contains(this))
			instance.ActiveManagers.Add(this);
		else throw new System.Exception("Duplicate Manager");
	}
}