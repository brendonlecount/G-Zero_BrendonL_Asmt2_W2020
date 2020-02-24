using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script implements a singleton that manages a dictionary of hovercraft variants. The original
// plan was to have multiple hovercraft that the player could pick from at the start, but that was scrapped
// due to time constraints (it's not as much fun without those cool stat graphs that the original F-Zero had,
// and those looked like they'd take a while to implement.) The real dictionary part of this assignment is
// in the zone data manager.


public class HovercraftStatsManager : MonoBehaviour
{
	[SerializeField] private HovercraftStats[] hovercraftStatsList;


	private static HovercraftStatsManager instance = null;

	private static Dictionary<string, HovercraftStats> hovercraftStats = new Dictionary<string, HovercraftStats>();

	private void Awake()
	{
		if (instance == null)
		{
			foreach(HovercraftStats stats in hovercraftStatsList)
			{
				hovercraftStats.Add(stats.VariantTag, stats);
			}
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public static HovercraftStats GetHovercraftStats(string variantTag)
	{
		return hovercraftStats[variantTag];
	}

	public static HovercraftStats GetDefaultStats()
	{
		return instance.hovercraftStatsList[0];
	}
}
