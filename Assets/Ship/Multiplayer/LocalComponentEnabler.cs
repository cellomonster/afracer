using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Ship.Multiplayer
{


    public class LocalComponentEnabler : NetworkBehaviour
    {
        [SerializeField] private MonoBehaviour[] localOnlyComponents;

		public override void OnNetworkSpawn()
		{
			// disable non-local components
			foreach(MonoBehaviour component in localOnlyComponents)
			{
				component.enabled = IsLocalPlayer;
			}


		}
	}
}