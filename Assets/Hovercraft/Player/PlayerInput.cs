using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script uses player input to drive a Hovercraft character controller.

[RequireComponent(typeof(Hovercraft))]
public class PlayerInput : MonoBehaviour
{
	private static PlayerInput instance = null;
	public static PlayerInput Instance => instance;

	public Hovercraft Hovercraft { get; private set; }

	private void Awake()
	{
		Hovercraft = GetComponent<Hovercraft>();
		instance = this;
	}


	private void Update()
	{
		if (RaceManager.Instance.RaceMode == RaceMode.Racing)
		{
			if (Input.GetButtonDown("Jump"))
			{
				Hovercraft.TurnBoost = true;
			}
			else if (Input.GetButtonUp("Jump"))
			{
				Hovercraft.TurnBoost = false;
			}

			if (Input.GetButtonDown("Boost"))
			{
				Hovercraft.TriggerBoost();
			}

			Hovercraft.AccelAxis = Input.GetAxis("Vertical");
			Hovercraft.TurnAxis = Input.GetAxis("Horizontal");
		}
		else
		{
			Hovercraft.TurnBoost = false;
			Hovercraft.AccelAxis = 0f;
			Hovercraft.TurnAxis = 0f;
		}
	}

}
