using UnityEngine;

namespace Race
{
	[CreateAssetMenu(fileName = "RaceSettings", menuName = "ScriptableObjects/RaceSettings")]
	public class RaceSettings : ScriptableObject
	{
		public int laps = 3;
		public bool localPlay = true;
		public int speedClass = 0;

		public void SetLaps(int laps)
		{
			this.laps = laps;
		}

		public void SetLocalPlay(bool localPlay)
		{
			this.localPlay = localPlay;
		}

		public void SetSpeedClass(int speedClass)
		{
			this.speedClass = speedClass;
		}
	}
}
