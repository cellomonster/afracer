using System;
using UnityEngine;

namespace Deprecated
{
	[RequireComponent(typeof(Rigidbody))]
	public class Levitator : MonoBehaviour
	{
		[SerializeField] private float hoverHeight = 1;
		[SerializeField] private float forceMultiplier = 1;
		[SerializeField] private PIDController pidController;
		[SerializeField] private Vector3 positionLocal;
		[SerializeField] private LayerMask groundRayMask;

		[SerializeField] private new Rigidbody rigidbody;

		public bool IsGrounded { get; private set; }

		private void FixedUpdate()
		{
			Vector3 rayOrigin = transform.TransformPoint(positionLocal);
			Ray groundRay = new Ray(rayOrigin, -Vector3.up);
			IsGrounded = Physics.Raycast(groundRay, out var hit, float.MaxValue, groundRayMask);

			if (IsGrounded && hit.distance < hoverHeight)
			{
				float pidMagnitude = pidController.Update(hoverHeight - hit.distance, 1.0f);
				//pidMagnitude = Mathf.Clamp(pidMagnitude, -1, 1);

				if (pidMagnitude < 0)
					return;

				rigidbody.AddForceAtPosition(Vector3.up * pidMagnitude * forceMultiplier, rayOrigin);

			}
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (rigidbody == null)
			{
				rigidbody = GetComponent<Rigidbody>();
			}
		}

		private void OnDrawGizmosSelected()
		{
			Vector3 position = transform.TransformPoint(positionLocal);
			Vector3 endPosition = position - Vector3.up * hoverHeight;

			Gizmos.color = Color.blue;
			Gizmos.DrawLine(position, endPosition);
		}
#endif
	}
}