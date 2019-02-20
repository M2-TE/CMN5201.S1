using System.Collections.Generic;
using UnityEngine.Experimental.Input;

public class InputManager : Manager
{
	public readonly InputMaster Input;
	private readonly Dictionary<object, Dictionary<InputAction, List<System.Action<InputAction.CallbackContext>>>> perObjectCallbacks;

	public InputManager(InputMaster inputMaster)
	{
		perObjectCallbacks = new Dictionary<object, Dictionary<InputAction, List<System.Action<InputAction.CallbackContext>>>>();
		Input = inputMaster;
		Input.Enable();
	}

	public void AddListener(InputAction action, System.Action<InputAction.CallbackContext> callback)
	{
		if (!perObjectCallbacks.ContainsKey(callback.Target)) // object's first callback registration	
			perObjectCallbacks.Add(callback.Target, new Dictionary<InputAction, List<System.Action<InputAction.CallbackContext>>>());
	
		if (!perObjectCallbacks[callback.Target].ContainsKey(action)) // object's first callback registration for this action
			perObjectCallbacks[callback.Target].Add(action, new List<System.Action<InputAction.CallbackContext>>());
		
		perObjectCallbacks[callback.Target][action].Add(callback);
		action.performed += callback;
	}

	public void RemoveListeners(object obj)
	{
		List<InputAction> registeredInputActions = new List<InputAction>(perObjectCallbacks[obj].Keys);
		List<System.Action<InputAction.CallbackContext>> callbacks;
		for (int i = 0; i < registeredInputActions.Count; i++)
		{
			callbacks = new List<System.Action<InputAction.CallbackContext>>(perObjectCallbacks[obj][registeredInputActions[i]]);

			for (int x = 0; x < callbacks.Count; x++)
				registeredInputActions[i].performed -= callbacks[x];
		}
	}

	// TODO move input buffering logic over to this new manager
}
