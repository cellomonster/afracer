using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
	[CreateAssetMenu(fileName = "Menu Actions", menuName = "ScriptableObjects/MenuActions")]
	public class MenuActions : ScriptableObject
	{
		public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
	}
}