using UnityEngine;

namespace _Scripts.Utils.SavableValues
{
	public class BoolDataValueSavable : DataValueSavable<bool>
	{
		public BoolDataValueSavable(string saveKey) : base(saveKey)
		{
		}

		protected override void Load()
		{
			base.Load();
			Value = PlayerPrefs.GetInt(SaveKey, 0) == 1;
		}

		public override void Save()
		{
			base.Save();
			PlayerPrefs.SetInt(SaveKey, Value ? 1 : 0);
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