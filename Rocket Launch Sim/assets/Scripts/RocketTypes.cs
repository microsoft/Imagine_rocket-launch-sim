using UnityEngine;
using System.Collections;

/// <summary>
/// A struct representing a rocket material.
/// </summary>
[System.Serializable]
public struct RocketMaterial
{
	public string materialKey; // This is a localization key
	public int cost;
	public int weight;
}

/// <summary>
/// A struct representing a fuel type.
/// </summary>
[System.Serializable]
public struct RocketFuel
{
	public string fuelKey; // This is a localization key
	public int costPerWeight;
	public float impulsePerWeight;
}