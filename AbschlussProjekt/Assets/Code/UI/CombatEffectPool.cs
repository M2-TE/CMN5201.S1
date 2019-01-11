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

		private Queue<CombatEffectUI> inactiveCombatEffectElements;
		public List<CombatEffectUI> activeCombatEffectElements;
		#endregion
		
		private void Start()
		{
			inactiveCombatEffectElements = new Queue<CombatEffectUI>();
			activeCombatEffectElements = new List<CombatEffectUI>();
		}

		#region Pooling
		private CombatEffectUI CreateNewElement()
		{
			CombatEffectUI item = Instantiate(combatEffectPrefab, transform).GetComponent<CombatEffectUI>();

			item.transform.GetChild(0).gameObject.SetActive(showDuration);
			item.SetSize(effectSize.x, effectSize.y);

			return item;
		}

		private CombatEffectUI GetItemFromPool()
		{
			CombatEffectUI item = null;
			if (inactiveCombatEffectElements.Count > 0) item = inactiveCombatEffectElements.Dequeue();
			else item = CreateNewElement();

			item.SetPosition(GetNextFreePosition());
			activeCombatEffectElements.Add(item);
			item.gameObject.SetActive(true);

			return item;
		}

		private void StoreItemInPool(CombatEffectUI item)
		{
			item.gameObject.SetActive(false);
			activeCombatEffectElements.Remove(item);
			inactiveCombatEffectElements.Enqueue(item);
		}
		#endregion
		
		private Vector2 GetNextFreePosition(int startOffset = 0)
		{
			float xPos = (activeCombatEffectElements.Count + startOffset) % displacementRollover * displacement.x;
			float yPos = displacement.y * ((activeCombatEffectElements.Count + startOffset) / displacementRollover);
			return new Vector2(xPos, yPos);
		}

		private CombatEffectUI FindCombatEffectElement(CombatEffect combatEffect)
		{
			return activeCombatEffectElements.Find(x => x.CombatEffect == combatEffect);
		}

		#region Public Methods
		public CombatEffectUI AddCombatEffect(CombatEffect combatEffect)
		{
			CombatEffectUI item = GetItemFromPool();
			item.CombatEffect = combatEffect;
			item.Duration = combatEffect.Duration;
			return item;
		}
		
		public void RemoveCombatEffect(CombatEffect combatEffect)
		{
			RemoveCombatEffect(FindCombatEffectElement(combatEffect));
		}
		public void RemoveCombatEffect(CombatEffectUI combatEffect)
		{
			StoreItemInPool(combatEffect);
			// rearrange existing combat effects
			for(int i = 0; i < activeCombatEffectElements.Count; i++)
				activeCombatEffectElements[i].transform.localPosition = GetNextFreePosition(-activeCombatEffectElements.Count + i);
		}

		public bool Contains(CombatEffect combatEffect)
		{
			return (FindCombatEffectElement(combatEffect) != null);
		}

		public void CountdownCombatEffectDurations(int countdownVal = -1)
		{
			// TODO
		}

		public void CopyCombatEffects(CombatEffectPool pool)
		{
			for (int i = 0; i < pool.activeCombatEffectElements.Count || i < activeCombatEffectElements.Count; i++)
			{
				// use current reserves of active combatEffect elements for copy process
				if (i < activeCombatEffectElements.Count && i < pool.activeCombatEffectElements.Count)
				{
					activeCombatEffectElements[i].CombatEffect = pool.activeCombatEffectElements[i].CombatEffect;
					activeCombatEffectElements[i].Duration = pool.activeCombatEffectElements[i].Duration;
				}
				// get new items from the pool if there are more effects left to be copied
				else if (activeCombatEffectElements.Count < pool.activeCombatEffectElements.Count)
				{
					CombatEffectUI combatEffect = GetItemFromPool();
					combatEffect.CombatEffect = pool.activeCombatEffectElements[i].CombatEffect;
					combatEffect.Duration = pool.activeCombatEffectElements[i].Duration;
				}
				// pool leftover effect objects that are not being used
				else
				{
					RemoveCombatEffect(activeCombatEffectElements[pool.activeCombatEffectElements.Count]);
					i--;
				}
			}
		}
		#endregion
	}
}
