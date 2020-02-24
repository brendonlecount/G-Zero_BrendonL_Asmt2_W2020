using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Brendon LeCount 2/11/2020
// This script manages the HUD race start countdown UI element

public class Countdown : MonoBehaviour
{
	[SerializeField] private Text countdownText;
	[SerializeField] private AudioSource countdownSource;
	[SerializeField] private AudioSource finalSource;

	private static Countdown instance;

	private void Awake()
	{
		instance = this;
		countdownText.gameObject.SetActive(false);
	}

	public static void ShowCountdown(int stage)
	{
		instance.countdownText.text = stage.ToString("N0");
		instance.countdownText.gameObject.SetActive(true);
		instance.countdownSource.Play();
	}

	public static void ShowGo()
	{
		instance.countdownText.text = "GO!!";
		instance.countdownText.gameObject.SetActive(true);
		instance.finalSource.Play();
	}

	public static void Hide()
	{
		instance.countdownText.gameObject.SetActive(false);
	}
}
