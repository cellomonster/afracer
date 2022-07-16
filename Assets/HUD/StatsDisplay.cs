using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Player;
using Race;
using TMPro;

namespace HUD
{
	public class StatsDisplay : MonoBehaviour
	{
		[SerializeField] private RaceSettings raceSettings;

		[SerializeField] private TextMeshProUGUI lapsDisplay;
		[SerializeField] private TextMeshProUGUI timeDisplay;

		private RaceTracker tracker;
		private bool raceStarted = false;

		private StartCountdown startCountdown;

		private void Awake()
		{
			OnLap(0, 0);
			timeDisplay.text = "0:00";

			FindObjectOfType<StartCountdown>().OnCountdownFinish += delegate
			{
				raceStarted = true;
			};
		}

		public void Subscribe(RaceTracker tracker)
		{
			this.tracker = tracker;
			tracker.OnLap += OnLap;
		}

		private void OnLap(int finishedLaps, float lapTime)
		{
			lapsDisplay.text = (finishedLaps + 1) + "/" + raceSettings.laps;
		}

		private void Update()
		{
			if (raceStarted)
			{
				timeDisplay.text = tracker.CurrentLapTime.ToString(CultureInfo.CurrentCulture);
			}
		}
	}
}