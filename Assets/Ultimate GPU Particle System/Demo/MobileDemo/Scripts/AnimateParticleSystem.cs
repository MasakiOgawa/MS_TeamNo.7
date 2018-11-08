using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateParticleSystem : MonoBehaviour
{
	public float speed = 5f;

	void Start () {
		Application.targetFrameRate = 60;
	}
	
	void Update ()
	{
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit = new RaycastHit();

			if (Physics.Raycast(ray, out rayHit))
			{
				transform.position = rayHit.point;
			}
		}
		else {
			transform.position = new Vector3(Mathf.Sin(Time.time * speed) * 16f, Mathf.Sin(Time.time * 2f * speed) * 7f, 22f);
		}
	}
}
