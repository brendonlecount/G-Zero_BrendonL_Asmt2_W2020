using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script controls the camera. The camera follows whatever the PlayerInput singleton is
// attached to. Camera position is determined by the cameraOffset vector, and rotation is
// governed by the lookAheadAngle. It also manages wind noise that gets louder and higher
// in pitch as the camera moves faster (there should probably be a separate script for that...)

public class PlayerCamera : MonoBehaviour
{
	[SerializeField] private Vector3 cameraOffset;
	[SerializeField] private float lookAheadAngle;
	[SerializeField] private float lerpFactor;
	[SerializeField] private float rotationLerpFactor;

	[Header("Wind Noise")]
	[SerializeField] private AudioSource windSource;
	[SerializeField] private float windVolumeMin;
	[SerializeField] private float windVolumeMax;
	[SerializeField] private float windPitchMin;
	[SerializeField] private float windPitchMax;
	[SerializeField] private float speedMax;


	private Transform target = null;
	private Quaternion lookAheadRotation;

	float speed;

    void Start()
    {
		target = PlayerInput.Instance.transform;
		lookAheadRotation = Quaternion.Euler(-lookAheadAngle, 0f, 0f);
		transform.position = GetTargetPosition();
		transform.rotation = GetTargetRotation();
    }

    void FixedUpdate()
    {
		PositionCamera();
		OrientCamera();
		UpdateWindNoise();
    }

	private Vector3 GetTargetPosition()
	{
		return target.position + target.rotation * cameraOffset;
	}

	private Quaternion GetTargetRotation()
	{
		return Quaternion.LookRotation(target.position - transform.position, Vector3.up) * lookAheadRotation;
	}

	private void PositionCamera()
	{
		Vector3 lastPosition = transform.position;
		transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), lerpFactor * Time.deltaTime);
		speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
	}

	private void OrientCamera()
	{
		//		transform.LookAt(target);
		//		transform.Rotate(Vector3.left * lookAheadAngle, Space.Self);
		transform.rotation = Quaternion.Lerp(transform.rotation, GetTargetRotation(), rotationLerpFactor * Time.deltaTime);
	}

	private void UpdateWindNoise()
	{
		windSource.volume = Mathf.Lerp(windVolumeMin, windVolumeMax, speed / speedMax);
		windSource.pitch = Mathf.Lerp(windPitchMin, windPitchMax, speed / speedMax);
	}
}
