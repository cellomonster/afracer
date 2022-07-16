using System;
using Unity.Netcode;
using UnityEngine;

namespace Race
{
	public class LocalHostOnStart : MonoBehaviour
	{
		private void Start()
		{
			GetComponent<NetworkManager>().StartHost();
		}
	}
}
