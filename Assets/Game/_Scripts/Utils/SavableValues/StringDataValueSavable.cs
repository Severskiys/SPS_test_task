using UnityEngine;

namespace _Scripts.Utils.SavableValues
{
	public class StringDataValueSavable : DataValueSavable<string>
	{
		public StringDataValueSavable(string saveKey) : base(saveKey)
		{
		}

		protected override void Load()
		{
			base.Load();
			Value = PlayerPrefs.GetString(SaveKey, "");
		}

		public override void Save()
		{
			base.Save();
			PlayerPrefs.SetString(SaveKey, Value);
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