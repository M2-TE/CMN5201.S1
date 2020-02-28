using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenPanel : UIPanel
{
	public Image CoverImage;

	private void OnEnable()
	{
		(AssetManager.Instance.GetManager<LoadingScreenManager>() ?? new LoadingScreenManager()).Register(this);
	}
}
