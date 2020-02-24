using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Brendon LeCount 2/11/2020
// This script the health HUD readout UI element.

public class HealthReadout : MonoBehaviour
{
	[SerializeField] private float updateInterval;
	[SerializeField] private Text readout;
	[SerializeField] private Color deadColor;
	[SerializeField] private Color midColor;
	[SerializeField] private Color fullColor;

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
			float healthFraction = playerHovercraft.GetHealthFraction();
			readout.text = "Health: " + (100f * healthFraction).ToString("N0") + "%";
			if (healthFraction < 0.5f)
			{
				readout.color = Color.Lerp(deadColor, midColor, healthFraction / 0.5f);
			}
			else
			{
				readout.color = Color.Lerp(midColor, fullColor, (healthFraction - 0.5f) / 0.5f);
			}

			yield return new WaitForSeconds(updateInterval);
		}
	}
}
