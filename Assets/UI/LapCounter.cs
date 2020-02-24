using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Brendon LeCount 2/11/2020
// This script manages the lap counter UI element.

public class LapCounter : MonoBehaviour
{
	[SerializeField] private Text readout;
	[SerializeField] private AudioSource completionAudio;

	// Start is called before the first frame update
	void Start()
	{
		RaceManager.Instance.onLapCompleted += LapCompletedHandler;
		SetLapText(RaceManager.Instance.LapCount, RaceManager.Instance.TargetLaps);
	}

	private void LapCompletedHandler(int lapsCompleted, int targetLaps)
	{
		SetLapText(lapsCompleted, targetLaps);
		completionAudio.Play();
	}

	private void SetLapText(int lapsCompleted, int targetLaps)
	{
		readout.text = lapsCompleted.ToString("N0") + "/" + targetLaps.ToString("N0");
	}
}
