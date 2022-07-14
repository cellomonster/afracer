using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship
{
	[RequireComponent(typeof(Rigidbody))]
	public class Piloting : MonoBehaviour
	{

		[SerializeField] private float thrustForce = 40f;
		[SerializeField] private float steerTorque = 10f;
		[Header("air brakes")]
		[SerializeField] private float driftTorqueCoeff = 1f;
		[SerializeField] private float driftTorqueClamp = 20f;
		[SerializeField] private float driftForceCoeff = 5f;
		[SerializeField] private float brakeForceCoeff = 1f;
		[Header("air resistance")]
		[SerializeField] private Vector3 airResistanceCoeff = new Vector3(1, 0, 0);

		[SerializeField] private new Rigidbody rigidbody;

		private float thrustPower;
		private float steerPower;
		private float driftLeftPower;
		private float driftRightPower;

		public void Thrust(InputAction.CallbackContext context)
        {
			thrustPower = context.ReadValue<float>();
        }

		public void Steer(InputAction.CallbackContext context)
		{
			steerPower = context.ReadValue<Vector2>().x;
		}

		public void AirBrakeLeft(InputAction.CallbackContext context)
		{
			driftLeftPower = context.ReadValue<float>();
		}

		public void AirBrakeRight(InputAction.CallbackContext context)
		{
			driftRightPower = context.ReadValue<float>();
		}

		private void FixedUpdate()
		{
			Vector3 velocityLocal = transform.InverseTransformDirection(rigidbody.velocity);

			float driftTorquePower = driftRightPower - driftLeftPower;

			Vector3 force = Vector3.zero;
			float torqueY = 0;

			// thrust
			force += new Vector3(0, 0, thrustForce * thrustPower);

			// air brakes
			float brakePower = Mathf.Clamp01(driftRightPower + driftLeftPower - 1);
			float driftForceCoeffPower = driftForceCoeff * driftTorquePower;
			float brakeForceCoeffPower = brakeForceCoeff * brakePower;
			force -= new Vector3(driftForceCoeffPower, 0, brakeForceCoeffPower) * velocityLocal.z;

			// air resistance
			force -= Vector3.Scale(airResistanceCoeff, velocityLocal);

			// steer
			torqueY += steerTorque * steerPower;

			// air brake torque
			float driftTorque = driftTorqueCoeff * driftTorquePower * velocityLocal.z;
			torqueY += Mathf.Clamp(driftTorque, -driftTorqueClamp, driftTorqueClamp);

			Vector3 torque = new Vector3(0, torqueY, 0);

			rigidbody.AddRelativeTorque(torque);
			rigidbody.AddRelativeForce(force);

		}
	}
}
