using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	public class InputBuffer
	{
		private List<KeyCode> bufferedKeypresses; // float is remaining buffer duration
		private MonoBehaviour monoBehaviour;
		private readonly float maxBufferDuration;

		public InputBuffer(MonoBehaviour monoBehaviour, float maxBufferDuration)
		{
			this.monoBehaviour = monoBehaviour;
			this.maxBufferDuration = maxBufferDuration;

			bufferedKeypresses = new List<KeyCode>();
		}

		public void BufferInput(KeyCode bufferedKey)
		{
			if (!bufferedKeypresses.Contains(bufferedKey))
			{
				bufferedKeypresses.Add(bufferedKey);
				monoBehaviour.StartCoroutine(InitBufferRemoval(bufferedKey));
			}
		}

		public bool GetKey(KeyCode key)
		{
			return bufferedKeypresses.Remove(key);
		}

		private IEnumerator InitBufferRemoval(KeyCode keyToRemove)
		{
			yield return new WaitForSecondsRealtime(maxBufferDuration);
			bufferedKeypresses.Remove(keyToRemove);
		}
	}
}