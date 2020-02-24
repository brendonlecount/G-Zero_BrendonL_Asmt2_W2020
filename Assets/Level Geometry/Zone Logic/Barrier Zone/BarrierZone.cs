using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script contains logic and metadata for barrier zones that slow the player and apply damage.

[CreateAssetMenu(fileName = "Barrier Zone", menuName = "ScriptableObjects/Zones/Barrier Zone", order = 1)]
public class BarrierZone : Zone
{
	[SerializeField] private float damagePerSecond;
	[SerializeField] private float dragCoeff;

	public override void ApplyZone(Hovercraft hovercraft)
	{
		hovercraft.DamageHealth(damagePerSecond * Time.deltaTime);
		hovercraft.Rb.AddForce(-hovercraft.Rb.velocity * hovercraft.Rb.velocity.magnitude * dragCoeff);
	}

	public override string GetTag()
	{
		return "Barrier";
	}
}
