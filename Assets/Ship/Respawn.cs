using UnityEngine;

namespace Ship {
	public class Respawn : MonoBehaviour
	{
		[SerializeField] private Tracking tracking;
		[SerializeField] private new Rigidbody rigidbody;

		private void FixedUpdate()
		{
			if (rigidbody.position.y < -100)
			{
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;

				rigidbody.position = tracking.LastGroundedTrackPosition + Vector3.up;
				rigidbody.rotation = Quaternion.identity;

				transform.forward = tracking.LastGroundedTrackTangent;
			}
		}
	}
}
