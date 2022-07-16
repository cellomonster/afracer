using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Race
{
	public class RaceManager : MonoBehaviour
	{
		[SerializeField] private RaceSettings raceSettings;

		private NetworkManager networkManager;
		private StartCountdown startCountdown;

		private void Awake()
		{
			networkManager = GetComponent<NetworkManager>();
			startCountdown = GetComponent<StartCountdown>();
		}

		private void Start()
		{
			if(raceSettings.localPlay)
			{
				networkManager.StartHost();
				startCountdown.BeginCountdown(5);
			}
		}
	}
}