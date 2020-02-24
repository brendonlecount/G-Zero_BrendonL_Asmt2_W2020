using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Brendon LeCount 2/11/2020
// This script contains the speedometer UI readout.

public class Speedometer : MonoBehaviour
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
			readout.text = (playerHovercraft.GetSpeed() * 2.23694f).ToString("N0") + " mph";
			yield return new WaitForSeconds(updateInterval);
		}
	}
}
