using System;
using UnityEngine;

namespace Menu.Elements
{
	public class SelectionPrefix : MonoBehaviour
	{
		[SerializeField] private string prefix;

		private void Awake()
		{
			GetComponent<Selection>().UpdateTextMesh = i => prefix + i;
		}
	}
}