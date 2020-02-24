using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script contains logic and metadata for drag zones that slow the player.

[CreateAssetMenu(fileName = "Drag Zone", menuName = "ScriptableObjects/Zones/Drag Zone", order = 1)]
public class DragZone : Zone
{
	[SerializeField] private float dragCoeff;

	public override void ApplyZone(Hovercraft hovercraft)
	{
		hovercraft.Rb.AddForce(-hovercraft.Rb.velocity * hovercraft.Rb.velocity.magnitude * dragCoeff);
	}

	public override string GetTag()
	{
		return "Drag";
	}
}
