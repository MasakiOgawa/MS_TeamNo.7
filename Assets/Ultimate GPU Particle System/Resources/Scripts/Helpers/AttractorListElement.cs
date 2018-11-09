using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttractorListElement
{
	public Transform attractorPosition;
	public float attractionStrength = 1f;
	public float attenuation = 1f;
}
