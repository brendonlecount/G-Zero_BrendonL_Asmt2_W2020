using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script manages the "race finished" UI message.

public class FinishedUI : MonoBehaviour
{
	private static FinishedUI instance = null;

	private void Awake()
	{
		instance = this;
		gameObject.SetActive(false);
	}

	public static void Show()
	{
		instance.gameObject.SetActive(true);
	}
}
