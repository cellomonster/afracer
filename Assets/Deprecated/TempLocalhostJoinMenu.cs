using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TempLocalhostJoinMenu : MonoBehaviour
{
	[SerializeField] private NetworkManager networkManager;

	void OnGUI()
	{
		if (networkManager.IsServer || networkManager.IsHost || networkManager.IsConnectedClient)
			return;

		GUILayout.BeginArea(new Rect(0, 0, 100, 200));

		if (GUILayout.Button("Host"))
		{
			networkManager.StartHost();
		}
		else if (GUILayout.Button("Join"))
		{
			networkManager.StartClient();
		}


		GUILayout.EndArea();
	}
}