using Ship;
using Track;
using UnityEngine;
using UnityEngine.Splines;

namespace Player {
	public class Respawner : MonoBehaviour
	{
		
		private new Rigidbody rigidbody;
		private RaceTracker _raceTracker;
		private Levitator levitator;
		
		private Spline trackSpline;
		
		public Vector3 LastGroundedTrackPosition { get; private set; }
		public Vector3 LastGroundedTrackTangent { get; private set; }

		private void Awake()
		{
			trackSpline = FindObjectOfType<TrackTag>().Spline;
			
			levitator = GetComponent<Levitator>();
			rigidbody = GetComponent<Rigidbody>();
			_raceTracker = GetComponent<RaceTracker>();
		}

		private void FixedUpdate()
		{
			if (levitator.IsGrounded)
			{
				LastGroundedTrackPosition = _raceTracker.TrackPosition;
				LastGroundedTrackTangent = _raceTracker.TrackTangent;
			}
			
			if (rigidbody.position.y < -100)
			{
				RespawnAtLastValidPosition();
			}
		}

		public void RespawnAtLastValidPosition()
		{
			Respawn(LastGroundedTrackPosition, LastGroundedTrackTangent);
		}

		public void RespawnAtProgress(float progress)
		{
			Vector3 position = trackSpline.EvaluatePosition(progress);
			Vector3 forward = trackSpline.EvaluateTangent(progress);

			Respawn(position, forward);
		}

		private void Respawn(Vector3 position, Vector3 forward)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;

			rigidbody.position = position + Vector3.up;

			transform.forward = forward;
		}
	}
}
