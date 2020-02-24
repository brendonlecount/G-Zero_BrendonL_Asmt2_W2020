using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script is a hovercraft character controller. It can be driven via an AI script
// or user input via the TurnBoost, TurnAxis, and AccelAxis variables. The craft hovers
// above the ground, and if matchTerrainNormal is true it rotates to match the ground normal.
// Handling parameters and other metadata are obtained from HovercraftStats scriptable objects.

public class Hovercraft : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] private Rigidbody rb;
	[SerializeField] private SphereCollider sc;
	[SerializeField] private MeshRenderer mr;

	[Header("Hovering")]
	[SerializeField] private float hoverClearance;
	[SerializeField] private float hoverExtension;
	[SerializeField] private float hoverDamping;
	[SerializeField] private float hoverMaxGs;
	[SerializeField] private LayerMask hoverMask;
	[SerializeField] private bool matchTerrainNormal;

	[Header("Terrain Normal Matching")]
	[SerializeField] private float hoverCastRadius;
	[SerializeField] private float matchNormalTorque;
	[SerializeField] private float matchNormalDamping;
	[SerializeField] private RigidbodyConstraints matchNormalConstraints;

	[Header("Zone Interaction")]
	[SerializeField] private LayerMask zoneMask;
	[SerializeField] private float zoneDetectionDistance;
	[SerializeField] private float zoneDetectionBoxSize;

	[Header("Audio")]
	[SerializeField] private float soundUpdateInterval;
	[SerializeField] private AudioSource zoneAudio;
	[SerializeField] private AudioSource boosterAudio;
	[SerializeField] private AudioSource thrusterAudio;
	[SerializeField] private float thrusterVolumeMin;
	[SerializeField] private float thrusterVolumeMax;
	[SerializeField] private float thrusterFrequencyMin;
	[SerializeField] private float thrusterFrequencyMax;

	[Header("Other")]
	[SerializeField] private float boostTime;
	[SerializeField] private float bankAngle;
	[SerializeField] private float bankLerp;

	// Input
	[HideInInspector] public bool TurnBoost = false;
	[HideInInspector] public float TurnAxis = 0f;
	[HideInInspector] public float AccelAxis = 0f;

	public Rigidbody Rb => rb;

	// banking
	private float currentBankAngle = 0f;

	// Hovering
	private float halfHeight;
	private float groundYLast;

	// Terrain following
	private Vector3 terrainNormal;
	private Quaternion[] castRotations;

	// Zone detection
	private Vector3 zoneDetectionHalfSize;

	// Environment
	private float gMult;

	// Components
	private HovercraftStats stats;
	private GameObject model = null;

	// stats
	private float health;
	private int boosts;

	private float boostTimer = 0f;
	Zone currentZone = null;
	string currentZoneTag = "";
	string newZoneTag = "";


	public void SetStats(HovercraftStats hovercraftStats)
	{
		// assign stats
		this.stats = hovercraftStats;

		// recalculate derived stats
		health = stats.MaxHealth;

		// Swap model
		if (model != null)
		{
			Destroy(model);
		}
		if (stats.Model != null)
		{
			model = GameObject.Instantiate(stats.Model, transform.position, transform.rotation, transform);
			mr.enabled = false;
		}
		else
		{
			mr.enabled = true;
		}
	}

	public void TriggerBoost()
	{
		if (boosts > 0)
		{
			boostTimer = boostTime;
			boosts--;
			boosterAudio.Play();
		}
	}

	public float GetSpeed()
	{
		return rb.velocity.magnitude;
	}

	public float GetMaxHealth()
	{
		if (stats != null)
		{
			return stats.MaxHealth;
		}
		return 0f;
	}

	public float GetHealthFraction()
	{
		if (stats != null)
		{
			return health / stats.MaxHealth;
		}
		else
		{
			return 1f;
		}
	}

	public int GetBoosts()
	{
		return boosts; 
	}

	public void AddBoost()
	{
		boosts++;
	}

	public void DamageHealth(float damage)
	{
		if (RaceManager.Instance.RaceMode == RaceMode.Racing && damage > 0f)
		{
			health -= damage;
			if (health < 0f)
			{
				RaceManager.Instance.PlayerDied();
			}
		}
	}

	public void RestoreHealth(float heals)
	{
		if (RaceManager.Instance.RaceMode == RaceMode.Racing && heals > 0f)
		{
			health = Mathf.Min(health + heals, stats.MaxHealth);
		}
	}

	private void Start()
	{
		SetStats(HovercraftStatsManager.GetDefaultStats());

		gMult =Physics.gravity.magnitude;

		// Set Hover Derived
		halfHeight = sc.radius / 2f;

		zoneDetectionHalfSize = new Vector3(zoneDetectionBoxSize / 2f, 0.1f, zoneDetectionBoxSize / 2f);

		if (matchTerrainNormal)
		{
			rb.constraints = matchNormalConstraints;
			castRotations = new Quaternion[3];
			castRotations[0] = Quaternion.identity;
			castRotations[1] = Quaternion.Euler(0f, 120f, 0f);
			castRotations[2] = Quaternion.Euler(0f, 240f, 0f);
		}

		StartCoroutine(SoundUpdateRoutine());
	}

	private void Update()
	{
		UpdateBank();
	}

	private void FixedUpdate()
	{
		Vector3 accel = GetThrustAccel() + GetBreakingAccel();
		Vector3 force = GetDragForce();
		if (matchTerrainNormal)
		{
			accel += GetHoverAccelNormalMatch();
			rb.AddTorque(GetTurnAccelNormalMatch(), ForceMode.Acceleration);
		}
		else
		{
			accel += GetHoverAccel();
			rb.AddTorque(GetTurnAccel(), ForceMode.Acceleration);
		}

		rb.AddForce(accel * gMult * rb.mass + force, ForceMode.Force);

		ApplyZone();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + terrainNormal * 3f);
	}

	private void UpdateBank()
	{
		float bankTarget = 0f;
		if (TurnBoost)
		{
			if (TurnAxis > 0f)
			{
				bankTarget = -bankAngle;
			}
			else if (TurnAxis < 0f)
			{
				bankTarget = bankAngle;
			}
		}
		currentBankAngle = Mathf.Lerp(currentBankAngle, bankTarget, bankLerp * Time.deltaTime);
		model.transform.localRotation = Quaternion.Euler(0f, 0f, currentBankAngle);
	}

	private Vector3 GetTurnAccel()
	{
		Vector3 currentTurnRate = new Vector3(0f, rb.angularVelocity.y, 0f);
		Vector3 targetTurnRate = GetTargetTurnRate();
		return (targetTurnRate - currentTurnRate) * stats.TurnTorqueCoeff;
	}

	private Vector3 GetTurnAccelNormalMatch()
	{
		Vector3 relativeAngularVelocity = Quaternion.Inverse(transform.rotation) * rb.angularVelocity;
		Vector3 currentTurnRate = new Vector3(0f, relativeAngularVelocity.y, 0f);
		Vector3 targetTurnRate = GetTargetTurnRate();
		Vector3 turnTorque = transform.rotation * (targetTurnRate - currentTurnRate) * stats.TurnTorqueCoeff;

		Vector3 rightingTorque = Vector3.Cross(transform.rotation * Vector3.up, terrainNormal) * matchNormalTorque;
		Vector3 rightingDamping = relativeAngularVelocity;
		rightingDamping.y = 0f;
		rightingDamping = transform.rotation * rightingDamping * matchNormalDamping;

		return turnTorque + rightingTorque - rightingDamping;
	}

	private Vector3 GetTargetTurnRate()
	{
		if (TurnBoost)
		{
			return Vector3.up * TurnAxis * stats.TurnRateBoost;
		}
		else
		{
			return Vector3.up * TurnAxis * stats.TurnRateBase;
		}
	}

	private Vector3 GetHoverAccel()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, halfHeight + hoverExtension, hoverMask))
		{
			float distance = hit.distance - halfHeight;
			float hoverThrust;
			if (distance < hoverClearance)
			{
				hoverThrust = Mathf.Lerp(hoverMaxGs, 1f, distance / hoverClearance);
			}
			else
			{
				hoverThrust = Mathf.Lerp(1f, 0f, (distance - hoverClearance) / (hoverExtension - hoverClearance));
			}
			float groundYSpeed = (hit.point.y - groundYLast) / Time.deltaTime;
			groundYLast = hit.point.y;
			hoverThrust -= (rb.velocity.y - groundYLast) * hoverDamping;
			return Vector3.up * hoverThrust;
		}
		else
		{
			return Vector3.zero;
		}
	}

	private Vector3 GetHoverAccelNormalMatch()
	{
		RaycastHit[] hits = new RaycastHit[3];
		float distance = 0f;
		float groundY = 0f;
		for (int i = 0; i < 3; i++)
		{
			if (!Physics.Raycast(transform.position + transform.rotation * castRotations[i] * Vector3.forward * hoverCastRadius, transform.rotation * Vector3.down, out hits[i], Mathf.Infinity, hoverMask))
			{
				terrainNormal = Vector3.up;
				return Vector3.zero;
			}
			distance += hits[i].distance - halfHeight;
			groundY += hits[i].point.y;
		}
		distance /= 3f;
		groundY /= 3f;
		if (distance < hoverExtension)
		{
			terrainNormal = Vector3.Cross(hits[0].point - hits[1].point, hits[0].point - hits[2].point).normalized;
			float hoverThrust;
			if (distance < hoverClearance)
			{
				hoverThrust = Mathf.Lerp(hoverMaxGs, 1f, distance / hoverClearance);
			}
			else
			{
				hoverThrust = Mathf.Lerp(1f, 0f, (distance - hoverClearance) / (hoverExtension - hoverClearance));
			}
			hoverThrust *= terrainNormal.y;     // equal to cos(theta)
			float groundYSpeed = (groundY - groundYLast) / Time.deltaTime;
			groundYLast = groundY;
			Vector3 relativeVelocity = Quaternion.Inverse(transform.rotation) * rb.velocity;
			hoverThrust -= (relativeVelocity.y - groundYSpeed) * hoverDamping * terrainNormal.y;
			return transform.rotation * Vector3.up * hoverThrust;
		}
		else
		{
			groundYLast = groundY;
			terrainNormal = Vector3.up;
			return Vector3.zero;
		}
	}

	private Vector3 GetDragForce()
	{
		Vector3 relativeVelocity = Quaternion.Inverse(transform.rotation) * rb.velocity;
		Vector3 lateralDrag = -new Vector3(relativeVelocity.x, 0f, 0f) * Mathf.Abs(relativeVelocity.x) * stats.LateralDragCoeff;
		Vector3 verticalDrag = -new Vector3(0f, relativeVelocity.y, 0f) * Mathf.Abs(relativeVelocity.y) * stats.VerticalDragCoeff;
		Vector3 axialDrag = -new Vector3(0f, 0f, relativeVelocity.z) * Mathf.Abs(relativeVelocity.z) * stats.AxialDragCoeff;
		return transform.rotation * (lateralDrag + verticalDrag + axialDrag);
	}

	private Vector3 GetThrustAccel()
	{
		if (boostTimer > 0f)
		{
			boostTimer -= Time.deltaTime;
			return transform.rotation * (stats.BoostThrust * Vector3.forward);
		}
		else if (AccelAxis <= 0f)
		{
			return Vector3.zero;
		}
		else
		{
			return transform.rotation * (AccelAxis * stats.Thrust * Vector3.forward);
		}
	}

	private Vector3 GetBreakingAccel()
	{
		if (AccelAxis >= 0f)
		{
			return Vector3.zero;
		}
		else
		{
			return AccelAxis * stats.BreakingThrust * rb.velocity.normalized;
		}
	}

	private void ApplyZone()
	{
		RaycastHit hit;
		if (Physics.BoxCast(transform.position, zoneDetectionHalfSize, transform.rotation * Vector3.down, out hit, transform.rotation, zoneDetectionDistance + halfHeight, zoneMask))
		{
			newZoneTag = hit.collider.tag;
		}
		else
		{
			newZoneTag = "";
		}
		if (!newZoneTag.Equals(currentZoneTag))
		{
			if (currentZone != null && currentZone.LoopSound)
			{
				zoneAudio.Stop();
			}
			currentZone = ZoneManager.GetZone(newZoneTag);
			currentZoneTag = newZoneTag;
			if (currentZone != null && currentZone.SoundFxClip != null)
			{
				zoneAudio.clip = currentZone.SoundFxClip;
				zoneAudio.loop = currentZone.LoopSound;
				zoneAudio.volume = currentZone.Volume;
				zoneAudio.Play();
			}
		}
		if (currentZone != null)
		{
			currentZone.ApplyZone(this);
		}
	}

	IEnumerator SoundUpdateRoutine()
	{
		while (true)
		{
			thrusterAudio.volume = Mathf.Lerp(thrusterVolumeMin, thrusterVolumeMax, Mathf.Abs(AccelAxis));
			thrusterAudio.pitch = Mathf.Lerp(thrusterFrequencyMin, thrusterFrequencyMax, Mathf.Abs(AccelAxis));
			yield return new WaitForSeconds(soundUpdateInterval);
		}
	}
}
