using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script contains logic and metadata for jump zones that launch the player into the air.

[CreateAssetMenu(fileName = "Jump Zone", menuName = "ScriptableObjects/Zones/Jump Zone", order = 1)]
public class JumpZone : Zone
{
	[SerializeField] private float jumpCoeff;

	public override void ApplyZone(Hovercraft hovercraft)
	{
		hovercraft.Rb.AddForce(Vector3.up * jumpCoeff * hovercraft.Rb.velocity.magnitude * Physics.gravity.magnitude, ForceMode.Acceleration);
	}

	public override string GetTag()
	{
		return "Jump";
	}
}
