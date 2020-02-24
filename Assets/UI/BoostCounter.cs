using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Brendon LeCount 2/11/2020
// This script manages the HUD boost counter UI element

public class BoostCounter : MonoBehaviour
{
	[SerializeField] private float updateInterval;
	[SerializeField] private Text readout;

	private Hovercraft playerHovercraft;

	// Start is called before the first frame update
	void Start()
	{
		playerHovercraft = PlayerInput.Instance.Hovercraft;
		StartCoroutine(UpdateRoutine());
	}

	private IEnumerator UpdateRoutine()
	{
		while (true)
		{
			readout.text = "Boosts: " + playerHovercraft.GetBoosts().ToString("N0");
			yield return new WaitForSeconds(updateInterval);
		}
	}
}
