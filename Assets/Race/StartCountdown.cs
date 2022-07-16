using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Race
{
	public class StartCountdown : MonoBehaviour
	{
		
		public Action<int> OnCountdownUpdate = delegate(int i) { };
		public Action OnCountdownFinish = delegate { };

		public void BeginCountdown(int startingFrom)
		{
			StartCoroutine(CountdownSeconds(startingFrom));
		}

		private IEnumerator CountdownSeconds(int countdown)
		{
			for (int i = countdown; i > 0; i--)
			{
				OnCountdownUpdate.Invoke(i);
				yield return new WaitForSeconds(1);
			}
			OnCountdownUpdate.Invoke(0);
			OnCountdownFinish.Invoke();
			yield return null;
		}
	}
}