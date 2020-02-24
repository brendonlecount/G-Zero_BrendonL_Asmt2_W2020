using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script contains logic and metadata for power zones that recharge the player's health.

[CreateAssetMenu(fileName = "Power Zone", menuName = "ScriptableObjects/Zones/Power Zone", order = 1)]
public class PowerZone : Zone
{
	[SerializeField] private float healsPerSecond;

	public override void ApplyZone(Hovercraft hovercraft)
	{
		hovercraft.RestoreHealth(healsPerSecond * hovercraft.GetMaxHealth() * Time.deltaTime);
	}

	public override string GetTag()
	{
		return "Power";
	}
}
