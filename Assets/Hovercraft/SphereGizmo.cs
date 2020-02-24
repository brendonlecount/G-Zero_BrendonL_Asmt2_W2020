using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This utility script draws a green spherical gizmo centered on whatever it's attached to, of
// the specified radius. (It was used to represent the hovercraft spherecollider radius when
// scaling the hovercraft model).

public class SphereGizmo : MonoBehaviour
{
	[SerializeField] private float radius;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
