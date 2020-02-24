using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script is the base class for zone scripts that contain logic and metadata for the
// various zones that make up the race track.

public abstract class Zone : ScriptableObject
{
	[SerializeField] private AudioClip soundFXClip;
	public AudioClip SoundFxClip => soundFXClip;

	[SerializeField] private bool loopSound;
	public bool LoopSound => loopSound;

	[SerializeField] private float volume;
	public float Volume => volume;

	public abstract void ApplyZone(Hovercraft hovercraft);
	public abstract string GetTag();
}
