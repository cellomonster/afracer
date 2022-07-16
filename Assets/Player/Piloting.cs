using Race;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship
{
	[RequireComponent(typeof(Rigidbody))]
	public class Piloting : MonoBehaviour
	{

		public float thrustForce = 100f;
		public float steerTorque = 10f;
		[Header("air brakes")]
		public float driftTorqueCoeff = 1f;
		public float driftTorqueClamp = 20f;
		public float driftForceCoeff = 5f;
		public float brakeForceCoeff = 1f;
		[Header("air resistance")]
		public Vector3 airResistanceCoeff = new Vector3(1, 0, 0);

		

		private new Rigidbody rigidbody;

		private float thrustPower;
		private float steerPower;
		private float driftLeftPower;
		private float driftRightPower;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

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
