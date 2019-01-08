﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatStartDebug : MonoBehaviour 
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			SceneManager.LoadScene("Main");
		}
	}
}
