using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script manages the finish line trigger.

public class FinishLine : MonoBehaviour
{
	private static FinishLine instance = null;
	public static FinishLine Instance => instance;

	private void Awake()
	{
		instance = this;
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			gameObject.SetActive(false);
			Midpoint.Instance.gameObject.SetActive(true);
			other.GetComponent<Hovercraft>().AddBoost();
			RaceManager.Instance.CountLap();
		}
	}
}
