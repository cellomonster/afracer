using System;
using UnityEngine;

namespace Ship
{
	public class AirResistance : MonoBehaviour
	{
		[SerializeField] private float sideResistance = 5f;
		[SerializeField] private float angularResistance = 0.5f;

		[SerializeField] private new Rigidbody rigidbody;

		private void FixedUpdate()
		{

			Vector3 flatRight = Vector3.ProjectOnPlane(transform.right, Vector3.up);

			Vector3 sideForce = Vector3.Project(rigidbody.velocity, flatRight) * -sideResistance;

			rigidbody.AddForce(sideForce);
			
			rigidbody.AddTorque(-rigidbody.angularVelocity * angularResistance);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (rigidbody == null)
				rigidbody = GetComponent<Rigidbody>();
		}
#endif
	}
}