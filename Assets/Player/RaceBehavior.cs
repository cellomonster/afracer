using System;
using Race;
using Ship;
using UnityEngine;

namespace Player
{
	public class RaceBehavior : MonoBehaviour
	{
		[SerializeField] private float speedClassThrustCoeff = 50f;
		[SerializeField] private RaceSettings raceSettings;

		private RaceTracker raceTracker;
		private Piloting piloting;
		private Respawner respawner;

		private void Awake()
		{
			piloting = GetComponent<Piloting>();
			raceTracker = GetComponent<RaceTracker>();
			respawner = GetComponent<Respawner>();
		}

		private void Start()
		{
			// piloting settup
			piloting.enabled = false;
			FindObjectOfType<StartCountdown>().OnCountdownFinish += delegate
			{
				piloting.enabled = true;
			};
			piloting.thrustForce += speedClassThrustCoeff * (raceSettings.speedClass - 1);

			// race tracking setup
			raceTracker.OnLap += delegate(int finishedLaps, float lapTime)
			{
				bool finishedRace = finishedLaps == raceSettings.laps;
				if (finishedRace)
				{
					piloting.enabled = false;
					FindObjectOfType<MenuManager>().SetVisible(true);
				}
			};

			// move ship to starting line
			respawner.RespawnAtProgress(0);
		}
	}
}