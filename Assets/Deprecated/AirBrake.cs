using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship
{
	public class AirBrake : MonoBehaviour
	{

		[SerializeField] private Vector3 velocityProportionalForceLocal = -Vector3.forward;

		[SerializeField] private Vector3 positionLocal = Vector3.zero;

		[SerializeField] private new Rigidbody rigidbody;

		public float Power;

		public void SetPower(float power)
		{
			this.Power = Mathf.Clamp01(power);
		}

		public void SetPower(InputAction.CallbackContext context)
		{
			SetPower(context.ReadValue<float>());
		}

		private void FixedUpdate()
		{
			if (Power == 0)
				return;

			Vector3 position = transform.TransformPoint(positionLocal);
			Vector3 velocityProportionalForce = transform.TransformVector(velocityProportionalForceLocal);

			float forwardVelocity = Vector3.Dot(rigidbody.velocity, transform.forward);

			Vector3 force = velocityProportionalForce * forwardVelocity * Power;

			rigidbody.AddForceAtPosition(force, position);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (rigidbody == null)
				rigidbody = GetComponent<Rigidbody>();
		}

		private void OnDrawGizmosSelected()
		{
			Vector3 position = transform.TransformPoint(positionLocal);

			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(position, Vector3.one * 0.2f);
		}
#endif
	}
}
