using System.Collections.Generic;
using System;
using HUD;
using Race;
using Ship;
using Track;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Player
{
	public class RaceTracker : MonoBehaviour
	{
		// refs
		private Spline trackSpline;

		// lap timer
		private float lapTimeOffset;
		private bool lapTimerRunning;

		public List<float> LapTimes { get; private set; } = new List<float>(3);

		public float CurrentLapTime {
			get {
				if (lapTimerRunning)
					return Time.time - lapTimeOffset;
				else
					return 0;
			}
		}

		// lap position
		public float LapProgress { get; private set; }
		public Vector3 TrackPosition { get; private set; }
		public Vector3 TrackTangent { get; private set; }

		// lap tracking
		private int actualLaps;
		private int finishedLaps;

		// events
		public Action<int, float> OnLap = delegate { };
		public Action<float[]> OnRaceFinish = delegate { };



		private void Awake()
		{
			trackSpline = FindObjectOfType<TrackTag>().Spline;

			FindObjectOfType<StatsDisplay>().Subscribe(this);

			FindObjectOfType<StartCountdown>().OnCountdownFinish += StartLapTimer;
		}

		private float previousLapProgress;
		private void FixedUpdate()
		{
			// before updating lap progress
			previousLapProgress = LapProgress;

			// get lap position
			SplineUtility.GetNearestPoint(trackSpline, transform.position, out float3 pos, out float t);
			TrackPosition = pos;
			LapProgress = t;
			TrackTangent = trackSpline.EvaluateTangent(LapProgress);

			// check for finish line cross
			float deltaLapProgress = previousLapProgress - LapProgress;
			bool crossedFinishLine = Mathf.Abs(deltaLapProgress) > 0.9f;
			if (crossedFinishLine)
			{
				// 'actual laps' decreases if the player crosses the
				// finish line from the wrong direction, preventing cheesing
				actualLaps += Math.Sign(deltaLapProgress);

				if (actualLaps > finishedLaps)
				{
					finishedLaps = actualLaps;
					OnLap.Invoke(finishedLaps, CurrentLapTime);

					LapTimes.Add(CurrentLapTime);
					StartLapTimer();
				}
			}
		}

		private void StartLapTimer()
		{
			lapTimeOffset = Time.time;
			lapTimerRunning = true;
		}

		private void StopLapTimer()
		{
			lapTimerRunning = false;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(TrackPosition, 1f);
		}
	}
}