using UnityEngine;

namespace _Scripts.Utils.SavableValues
{
	public class IntDataValueSavable : DataValueSavable<int>
	{
		public IntDataValueSavable(string saveKey) : base(saveKey)
		{
		}

		protected override void Load()
		{
			base.Load();
			Value = PlayerPrefs.GetInt(SaveKey, 0);
		}

		public override void Save()
		{
			base.Save();
			PlayerPrefs.SetInt(SaveKey, Value);
		}

		public override void Delete()
		{
			base.Delete();
			PlayerPrefs.DeleteKey(SaveKey);
		}

		public override bool HasSaving()
		{
			return PlayerPrefs.HasKey(SaveKey);
		}
	}
}