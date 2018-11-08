using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
	public Transform cameraTransform;
	public Transform rotateTransform;
	public Transform zoomTransform;
	public float rotationSpeed = 5f;
	public float zoomSpeed = 5f;
	public float camSpeed = 50f;

	void Update ()
	{
		if (Input.mousePosition.x < 50f && Input.mousePosition.x > 0f)
		{
			rotateTransform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
		}

		if (Input.mousePosition.x > Screen.width - 50f && Input.mousePosition.x < Screen.width)
		{
			rotateTransform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
		}

		if (Input.mousePosition.y < 50f && Input.mousePosition.y > 0)
		{
			rotateTransform.Rotate(-Vector3.right * rotationSpeed * Time.deltaTime);
		}

		if (Input.mousePosition.y > Screen.height - 50f && Input.mousePosition.y < Screen.height)
		{
			rotateTransform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
		}

		if (Input.mouseScrollDelta.y < 0f)
		{
			zoomTransform.Translate(-Vector3.forward * zoomSpeed * Time.deltaTime);
		}

		if (Input.mouseScrollDelta.y > 0f)
		{
			zoomTransform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
		}

		if (zoomTransform.localPosition.z > -35f)
		{
			zoomTransform.localPosition = new Vector3(0f,0f,-35f);
		}

		if (zoomTransform.localPosition.z < -200f)
		{
			zoomTransform.localPosition = new Vector3(0f, 0f, -200f);
		}

		cameraTransform.position = Vector3.Lerp(cameraTransform.position, zoomTransform.position, camSpeed * Time.deltaTime);
		cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, zoomTransform.rotation, camSpeed * Time.deltaTime);//Quaternion.LookRotation(Vector3.Normalize(rotateTransform.position - cameraTransform.position));
	}
}
