using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages the Midpoint trigger - the player must reach the midpoint
// before the finish line trigger is enabled, to keep the player from just reversing
// back over the finish line repeatedly to finish their laps.

public class Midpoint : MonoBehaviour
{
	private static Midpoint instance = null;
	public static Midpoint Instance => instance;

	private void Awake()
	{
		instance = this;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			gameObject.SetActive(false);
			FinishLine.Instance.gameObject.SetActive(true);
		}
	}
}
