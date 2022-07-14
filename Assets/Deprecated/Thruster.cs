using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ship
{
	public class Thruster : MonoBehaviour
	{
		[SerializeField] private float forceMagnitude = 10;

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

			Vector3 thrusterPosition = transform.TransformPoint(positionLocal);

			Vector3 force = transform.forward * forceMagnitude * Power;

			rigidbody.AddForceAtPosition(force, thrusterPosition);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (rigidbody == null)
				rigidbody = GetComponent<Rigidbody>();
		}

        private void OnDrawGizmosSelected()
        {
			Vector3 thrusterPosition = transform.TransformPoint(positionLocal);

			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(thrusterPosition, Vector3.one * 0.2f);
        }
#endif
    }
}
