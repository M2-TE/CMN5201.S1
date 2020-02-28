using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatEffectElements
{
	[RequireComponent(typeof(Image))]
	public class CombatEffectUI : MonoBehaviour
	{
		#region Variables
		[SerializeField] private TextMeshProUGUI durationText;
		[SerializeField] private RectTransform rectTrans;
		[SerializeField] private Image imageComp;

		private int duration;
		public int Duration
		{
			get { return duration; }
			set
			{
				duration = value;
				durationText.SetText(duration.ToString());
			}
		}

		private CombatEffect combatEffect;
		public CombatEffect CombatEffect
		{
			get { return combatEffect; }
			set
			{
				combatEffect = value;
				SetCombatImage(value.EffectSprite);
			}
		}
		#endregion

		private void SetCombatImage(Sprite effectSprite)
		{
			imageComp.sprite = effectSprite;
		}

		#region Public Methods
		public void SetSize(float width, float height)
		{
			rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		}

		public void SetPosition(float posX, float posY)
		{
			SetPosition(new Vector2(posX, posY));
		}
		public void SetPosition(Vector2 position)
		{
			rectTrans.anchoredPosition = position;
		}
		#endregion
	}
}
