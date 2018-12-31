using System.Collections.Generic;
using UnityEngine;

namespace CombatEffectElements
{
	public class CombatEffectPool : MonoBehaviour
	{
		#region Variables
		[SerializeField] private GameObject combatEffectPrefab;
		[SerializeField] private Vector2 effectSize;
		[SerializeField] private Vector2 displacement;
		[SerializeField] private int displacementRollover;
		[SerializeField] private bool showDuration = false;

		private Stack<CombatEffectUI> inactiveCombatEffectElements;
		public List<CombatEffectUI> activeCombatEffectElements;
		#endregion

		#region Unity Calls
		private void Start()
		{
			inactiveCombatEffectElements = new Stack<CombatEffectUI>();
			activeCombatEffectElements = new List<CombatEffectUI>();
		}
		#endregion

		#region Pooling
		private CombatEffectUI GetItemFromPool()
		{
			CombatEffectUI item = null;
			if (inactiveCombatEffectElements.Count > 0) item = inactiveCombatEffectElements.Pop();
			else item = Instantiate(combatEffectPrefab, transform).GetComponent<CombatEffectUI>();

			item.gameObject.SetActive(true);
			item.transform.GetChild(0).gameObject.SetActive(showDuration);
			item.SetSize(effectSize.x, effectSize.y);
			item.SetPosition(GetNextFreePosition());
			activeCombatEffectElements.Add(item);

			return item;
		}

		private void StoreItemInPool(CombatEffectUI item)
		{
			item.gameObject.SetActive(false);
			activeCombatEffectElements.Remove(item);
			inactiveCombatEffectElements.Push(item);
		}
		#endregion

		// TODO
		private Vector2 GetNextFreePosition()
		{
			float xPos = activeCombatEffectElements.Count % displacementRollover * displacement.x;
			float yPos = displacement.y * (activeCombatEffectElements.Count / displacementRollover);
			return new Vector2(xPos, yPos);
		}

		private CombatEffectUI GetCombatEffectElement(CombatEffect combatEffect)
		{
			return activeCombatEffectElements.Find(x => x.CombatEffect == combatEffect);
		}

		#region Public Methods
		public void AddCombatEffect(CombatEffect combatEffect)
		{
			CombatEffectUI item = GetItemFromPool();
			item.CombatEffect = combatEffect;
			item.Duration = combatEffect.Duration;
		}

		// Theoreticially not necessay \/
		public void RemoveCombatEffect(CombatEffect combatEffect)
		{
			StoreItemInPool(GetCombatEffectElement(combatEffect));
		}

		public bool Contains(CombatEffect combatEffect)
		{
			return (GetCombatEffectElement(combatEffect) != null);
		}

		public void CountdownCombatEffectDurations(int countdownVal = -1)
		{
			// TODO
		}

		public void CopyCombatEffects(CombatEffectPool pool)
		{

		}
		#endregion
	}
}
