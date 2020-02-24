using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Brendon LeCount 2/11/2020
// This script keeps track of a race, including time and lap count. It initiates
// the starting countdown sequence, and handles race completion and failure.

public enum RaceMode { Starting, Racing, Finished, Dead }

public class RaceManager : MonoBehaviour
{
	[SerializeField] private float resetDelay;
	[SerializeField] private float startDelay;
	[SerializeField] private int targetLaps;
	public int TargetLaps => targetLaps;

	public delegate void OnLapCompleted(int lapsCompleted, int targetLaps);
	public event OnLapCompleted onLapCompleted;


	public float RaceTime { get; private set; } = 0f;

	public RaceMode RaceMode { get; private set; } = RaceMode.Starting;

	public int LapCount { get; private set; } = 0;

	private static RaceManager instance = null;
	public static RaceManager Instance => instance;

	public void CountLap()
	{
		LapCount++;
		onLapCompleted?.Invoke(LapCount, TargetLaps);
		if (LapCount >= TargetLaps)
		{
			RaceMode = RaceMode.Finished;
			FinishedUI.Show();
			StartCoroutine(ResetRoutine());
		}
	}

	public void PlayerDied()
	{
		RaceMode = RaceMode.Dead;
		DeadUI.Show();
		StartCoroutine(ResetRoutine());
	}

	private void Awake()
	{
		instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(RaceDelayRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (RaceMode == RaceMode.Racing)
		{
			RaceTime += Time.deltaTime;
		}
    }

	IEnumerator RaceDelayRoutine()
	{
		Countdown.ShowCountdown(3);
		yield return new WaitForSeconds(startDelay / 3f);
		Countdown.ShowCountdown(2);
		yield return new WaitForSeconds(startDelay / 3f);
		Countdown.ShowCountdown(1);
		yield return new WaitForSeconds(startDelay / 3f);
		Countdown.ShowGo();
		RaceMode = RaceMode.Racing;
		yield return new WaitForSeconds(startDelay / 3f);
		Countdown.Hide();
	}

	IEnumerator ResetRoutine()
	{
		yield return new WaitForSeconds(resetDelay);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
