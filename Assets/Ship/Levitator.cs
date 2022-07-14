using System;
using UnityEngine;

namespace Ship
{
	[RequireComponent(typeof(Rigidbody))]
	public class Levitator : MonoBehaviour
	{
		[SerializeField] private float hoverHeight = 1;
		[SerializeField] private float rayLength = 2;
		[SerializeField] private float forceCoeff = 1;
		[SerializeField] private PIDController pidController;
		[SerializeField] private LayerMask groundRayMask;
		[SerializeField] private Vector3 rayOrigin;

		[SerializeField] private new Rigidbody rigidbody;

		public bool IsGrounded { get; private set; }

		private void FixedUpdate()
		{
			Vector3 rayOriginLocal = transform.TransformPoint(rayOrigin);
			Ray groundRay = new Ray(rayOriginLocal, -Vector3.up);
			bool didHit = Physics.Raycast(groundRay, out var hit, rayLength, groundRayMask);

			IsGrounded = didHit && hit.distance < hoverHeight;

			Vector3 normal = Vector3.up;

			if (IsGrounded)
			{
				float pidMagnitude = pidController.Update(hoverHeight - hit.distance, 1.0f);
				if (pidMagnitude < 0)
					return;

				rigidbody.AddForce(Vector3.up * pidMagnitude * forceCoeff);

				normal = hit.normal;
			}

			Vector3 flattenedForward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
			Quaternion targetRot = Quaternion.LookRotation(flattenedForward, normal);
			rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, targetRot, 0.1f);

			////Vector3 localAngular = transform.TransformDirection(rigidbody.angularVelocity);
			////localAngular.x = 0;
			////rigidbody.angularVelocity = transform.InverseTransformDirection(localAngular);

			//rigidbody.angularVelocity 
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
			Vector3 position = transform.TransformPoint(rayOrigin);
			Vector3 endPosition = position - Vector3.up * hoverHeight;

			Gizmos.color = Color.blue;
			Gizmos.DrawLine(position, endPosition);
		}
#endif
	}
}