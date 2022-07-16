using System;
using Race;
using TMPro;
using UnityEngine;

namespace HUD
{
	public class CountdownDisplay : MonoBehaviour
	{
		private TextMeshProUGUI countdownLabel;

		private void Awake()
		{

			countdownLabel = GetComponent<TextMeshProUGUI>();
			StartCountdown countdown = FindObjectOfType<StartCountdown>();
			
			countdownLabel.gameObject.SetActive(false);
				
			countdown.OnCountdownUpdate += delegate(int i)
			{
				if (!countdownLabel.gameObject.activeSelf)
				{
					countdownLabel.gameObject.SetActive(true);
				}
				
				countdownLabel.text = i.ToString();
			};
			
			countdown.OnCountdownFinish += delegate
			{
				countdownLabel.gameObject.SetActive(false);
			};
		}
	}
}
