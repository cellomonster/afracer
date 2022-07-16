using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		EventSystem eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(null);
		eventSystem.SetSelectedGameObject(this.gameObject);
	}
}
