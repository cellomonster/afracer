using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Track;

namespace Ship
{
    public class Tracking : MonoBehaviour
    {
        [SerializeField] private Levitator levitator;

        private Spline trackSpline;

        private float previousLapProgress;
        private float deltaLapProgress;
        private bool readyToLap;

        public float LapProgress { get; private set; }
        public Vector3 TrackPosition { get; private set; }
        public Vector3 TrackTangent { get; private set; }

        public Vector3 LastGroundedTrackPosition { get; private set; }
        public Vector3 LastGroundedTrackTangent { get; private set; }

        public int NumFinishedLaps { get; private set; }

        public Action OnLap = delegate { };

        private void Start()
        {
            OnLap += delegate
            {
                NumFinishedLaps++;
                readyToLap = false;
            };

            trackSpline = FindObjectOfType<TrackTag>().Spline;
        }

        private void FixedUpdate()
        {
            previousLapProgress = LapProgress;
            SplineUtility.GetNearestPoint(trackSpline, transform.position, out float3 pos, out float t);
            TrackPosition = pos;
            LapProgress = t;
            deltaLapProgress = previousLapProgress - LapProgress;

            TrackTangent = SplineUtility.EvaluateTangent(trackSpline, LapProgress);

            if (levitator.IsGrounded)
            {
                LastGroundedTrackPosition = TrackPosition;
                LastGroundedTrackTangent = TrackTangent;
            }

            if(deltaLapProgress > 0.5f)
            {
                readyToLap = true;
            } else if(deltaLapProgress < -0.5f)
            {
                readyToLap = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Finish") && readyToLap)
            {
                OnLap();
            }
        }

        private void OnGUI()
        {

            string stats = string.Format("Lap Progress: {0}\nLaps: {1}", LapProgress, NumFinishedLaps);

            GUILayout.BeginArea(new Rect(20, 20, Screen.width - 40, Screen.height - 40));
            GUILayout.Label(stats);
            GUILayout.EndArea();
        }

        private void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(TrackPosition, 1f);
        }
    }
}