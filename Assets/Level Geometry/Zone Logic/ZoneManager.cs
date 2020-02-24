using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This script manages a dictionary that can be used to look up zone scriptable objects
// by tag.

public class ZoneManager : MonoBehaviour
{
	[SerializeField] private Zone[] zoneList;

	private static ZoneManager instance = null;

	private static Dictionary<string, Zone> zones = new Dictionary<string, Zone>();

	private void Awake()
	{
		if (instance == null) 
		{
			instance = this;
			foreach (Zone zone in zoneList)
			{
				zones[zone.GetTag()] = zone;
			}
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public static Zone GetZone(string zoneTag)
	{
		Zone zone;
		if (zones.TryGetValue(zoneTag, out zone))
		{
			return zone;
		}
		return null;
	}
}
