using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterWalker : MonoBehaviour
{
	[SerializeField] private int maxSimChars = 5;
	[SerializeField] private Rect[] walkableAreas;
	[SerializeField] private bool displaceZ = false;

	[Space][SerializeField] private float yOffset;
	[SerializeField] private float minActionDuration, maxActionDuration, movementSpeed, walkingTimerModifier;
	
	private void Start()
	{
		Savestate savestate = AssetManager.Instance.Savestate;

		var availableChars = new List<Entity>();
		for (int i = 0; i < 4; i++)
			if(savestate.CurrentTeam[i] != null) availableChars.Add(savestate.CurrentTeam[i]);
		for (int i = 0; i < savestate.OwnedCharacters.Count; i++)
			if (availableChars.Count < maxSimChars) availableChars.Add(savestate.OwnedCharacters[i]);
		
		for(int charIndex = 0; charIndex < availableChars.Count; charIndex++)
		{
			Rect rect = walkableAreas[Random.Range(0, walkableAreas.Length)];
			float spawnPosX = Random.Range(rect.position.x, rect.position.x + rect.width);
			var spawnPos = new Vector3(spawnPosX, rect.position.y + yOffset, .0001f *- charIndex + (displaceZ ? rect.y * .001f : 0f));

			StartCoroutine(ManageProxy(Instantiate
				(availableChars[charIndex].CharDataContainer.Prefab, 
				spawnPos,
				Quaternion.identity), rect.x, rect.x + rect.width));
		}
	}

	private IEnumerator ManageProxy(GameObject proxyGO, float xMin, float xMax)
	{
		for (int i = 0; i < proxyGO.transform.childCount; i++)
			proxyGO.transform.GetChild(i).gameObject.SetActive(false);

		var anim = proxyGO.GetComponent<Animator>();
		var renderer = proxyGO.GetComponent<SpriteRenderer>();
		while (true) // phase iteration
		{
			bool? walkingLeft = Random.Range(0f, 1f) < .5f ? Random.Range(0f, 1f) < .5f : (bool?)null;
			var timer = Random.Range(minActionDuration, maxActionDuration) * (walkingLeft != null ? walkingTimerModifier : 1f);
			switch (walkingLeft)
			{
				case true:
				case false:
					renderer.flipX = (bool)walkingLeft;
					anim.SetFloat("Movespeed", 1f);
					break;

				case null:
					anim.SetFloat("Movespeed", 0f);
					break;
			}

			while (true) // phase handling
			{
				if (timer < 0f) break;
				else timer -= Time.deltaTime;

				if (proxyGO.transform.position.x < xMin || proxyGO.transform.position.x > xMax)
				{
					walkingLeft = !walkingLeft;
					renderer.flipX = walkingLeft != null ? (bool)walkingLeft : true;
				}
				switch (walkingLeft)
				{
					case true:
						proxyGO.transform.Translate(new Vector3(-movementSpeed * Time.deltaTime, 0f, 0f));
						break;

					case false:
							proxyGO.transform.Translate(new Vector3(movementSpeed * Time.deltaTime, 0f, 0f));
						break;

					case null:
						break;
				}
				yield return null;
			}
			yield return null;
		}
	}

	private void OnDrawGizmos()
	{
		for(int i = 0; i < walkableAreas.Length; i++)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(walkableAreas[i].position, walkableAreas[i].position + new Vector2(walkableAreas[i].width, 0));
		}
	}
}
