using TMPro;
using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Menu.Elements
{
	public class Selection : MonoBehaviour, IMoveHandler
	{
		[SerializeField] private int selectionIndex = 1;
		[SerializeField] private int minSelection = 1;
		[SerializeField] private int maxSelection = 3;

		private TextMeshProUGUI textMesh;

		[SerializeField] private UnityEvent<int> onSelectionChange;

		public Func<int, string> UpdateTextMesh = i => i.ToString();

		private void Awake()
		{
			textMesh = GetComponent<TextMeshProUGUI>();
		}

		private void Start()
		{
			ChangeSelection(selectionIndex);
		}

		public void IncrementSelection()
		{
			ChangeSelection(++selectionIndex);
		}

		public void DecrementSelection()
		{
			ChangeSelection(--selectionIndex);
		}

		private void ChangeSelection(int i)
		{
			selectionIndex = Math.Clamp(i, minSelection, maxSelection);
			textMesh.text = UpdateTextMesh(selectionIndex);
			onSelectionChange.Invoke(selectionIndex);
		}

		public void OnMove(AxisEventData eventData)
		{
			ChangeSelection(selectionIndex + Math.Sign(eventData.moveVector.x));
		}
	}
}
