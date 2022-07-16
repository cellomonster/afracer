using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
	public class MenuManager : MonoBehaviour
	{
		[SerializeField] private bool hiddenByDefault;

		private Stack<GameObject> menuStack = new();

		private void Start()
		{
			for (int i = 1; i < transform.childCount; i++)
			{
				Transform t = transform.GetChild(i);

				t.gameObject.SetActive(false);
			}

			GameObject g = transform.GetChild(0).gameObject;
			g.SetActive(true);
			menuStack.Push(g);

			SetVisible(!hiddenByDefault);
		}

		public void SwitchMenu(GameObject page)
		{
			menuStack.Peek().SetActive(false);
			page.SetActive(true);
			menuStack.Push(page);

			FocusSelection();
		}

		public void Back()
		{
			if (menuStack.Count < 2)
				return;

			menuStack.Pop().SetActive(false);
			menuStack.Peek().SetActive(true);

			FocusSelection();
		}

		private void FocusSelection()
		{
			Selectable s = menuStack.Peek().GetComponentInChildren<Selectable>();

			EventSystem eventSystem = EventSystem.current;
			eventSystem.SetSelectedGameObject(null);
			eventSystem.SetSelectedGameObject(s.gameObject);
		}

		public void SetVisible(bool visible)
		{
			menuStack.Peek().SetActive(visible);

			if (visible)
			{
				FocusSelection();
			}
		}
	}
}