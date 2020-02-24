using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script contains stats and metadata for hovercraft, for use by the Hovercraft character controller.

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Hovercraft Stats", order = 1)]
public class HovercraftStats : ScriptableObject
{
	[Header("Identifiers")]
	[SerializeField] private string variantTag;
	public string VariantTag => variantTag;

	[SerializeField] private string displayName;
	public string DisplayName => displayName;

	[Header("Chassis")]
	[SerializeField] private float maxHealth;
	public float MaxHealth => maxHealth;

	[Header("Drag")]
	[SerializeField] private float lateralDragCoeff;
	public float LateralDragCoeff => lateralDragCoeff;

	[SerializeField] private float verticalDragCoeff;
	public float VerticalDragCoeff => verticalDragCoeff;

	[SerializeField] private float axialDragCoeff;
	public float AxialDragCoeff => axialDragCoeff;

	[Header("Thrust")]
	[SerializeField] private float thrust;
	public float Thrust => thrust;

	[SerializeField] private float breakingThrust;
	public float BreakingThrust => breakingThrust;

	[SerializeField] private float boostThrust;
	public float BoostThrust => boostThrust;

	[Header("Turning")]
	[SerializeField] private float turnRateBase;
	public float TurnRateBase => turnRateBase;

	[SerializeField] private float turnRateBoost;
	public float TurnRateBoost => turnRateBoost;

	[SerializeField] private float turnTorqueCoeff;
	public float TurnTorqueCoeff => turnTorqueCoeff;

	[Header("Visualization")]
	[SerializeField] private GameObject model;
	public GameObject Model => model;

}
