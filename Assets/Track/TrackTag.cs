using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace Track
{
	public class TrackTag : MonoBehaviour
	{
		[SerializeField] private SplineContainer splineContainer;

		public Spline Spline => splineContainer.Spline;

#if UNITY_EDITOR
		private void OnValidate()
		{
			gameObject.layer = LayerMask.NameToLayer("track");
		}
#endif
	}
}