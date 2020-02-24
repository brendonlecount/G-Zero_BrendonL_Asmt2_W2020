using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkEffectDriver : MonoBehaviour
{
	[SerializeField] private ParticleSystem ps;
	[SerializeField] private float rate;
	[SerializeField] private float duration;

	private ParticleSystem.EmissionModule em;
	private Coroutine stopSparksRoutine = null;

	private void Start()
	{
		em = ps.emission;
		em.rateOverTime = 0f;
		em.rateOverDistance = 0f;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Ground"))
		{
			if (stopSparksRoutine != null)
			{
				StopCoroutine(stopSparksRoutine);
			}
			em.rateOverDistance = rate;
			stopSparksRoutine = StartCoroutine(StopSparksRoutine());
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.collider.CompareTag("Ground"))
		{
			//em.rateOverDistance = 0f;
		}
	}

	public IEnumerator StopSparksRoutine()
	{
		for (int i = 0; i < 10; i++)
		{
			yield return new WaitForSeconds(duration / 10f);
			em.rateOverDistance = Mathf.Lerp(rate, 0f, (float)i / 10f);
		}
		em.rateOverDistance = 0f;
		stopSparksRoutine = null;
	}
}
