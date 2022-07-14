using System;
using UnityEngine;

namespace Cam
{
	[RequireComponent(typeof(Camera))]
	public class Follow : MonoBehaviour
	{
		[SerializeField] private Transform followTransform;
		[SerializeField] private bool follow;

		[SerializeField] private Vector3 offset = new(0, 3, -10);
		[SerializeField] private float lerpStrength = 0.5f;

		public void SetFollowTarget(Transform target)
		{
			followTransform = target;
			follow = true;
		}

		private void Update()
		{
			if (!follow)
				return;

			float lerpStrengthDT = lerpStrength * Time.deltaTime * 60;

			Vector3 lerpTarget = followTransform.position + Vector3.ProjectOnPlane(transform.forward, Vector3.up) * offset.z + Vector3.up * offset.y;
			transform.position = Vector3.Lerp(transform.position, lerpTarget, lerpStrengthDT);

			Quaternion rotTarget = Quaternion.Euler(0, followTransform.eulerAngles.y, 0);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotTarget, lerpStrengthDT);


		}
	}
}