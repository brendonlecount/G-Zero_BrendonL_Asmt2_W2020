using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Brendon LeCount 2/11/2020
// This script manages the race timer UI readout.

public class RaceTime : MonoBehaviour
{
	[SerializeField] private float updateInterval;
	[SerializeField] private Text readout;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(UpdateRoutine());
	}

	private IEnumerator UpdateRoutine()
	{
		while (true)
		{
			float seconds = RaceManager.Instance.RaceTime;
			readout.text = string.Format("{0:00}:{1:00}:{2:00.00}", seconds / 3600, (seconds / 60) % 60, seconds % 60);
			yield return new WaitForSeconds(updateInterval);
		}
	}
}
