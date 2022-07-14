using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    public class FollowMe : MonoBehaviour
    {
		[SerializeField] private bool followOnStart;

        private Follow follow;

		private void Start()
		{
			follow = Camera.main.GetComponent<Follow>();

			if(followOnStart)
			{
				Follow();
			}
		}

		public void Follow()
		{
			follow.SetFollowTarget(transform);
		}
    }
}