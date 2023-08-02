using UnityEngine;

namespace _Scripts.Utils.SavableValues
{
	public class FloatDataValueSavable : DataValueSavable<float>
	{
		public FloatDataValueSavable(string saveKey) : base(saveKey)
		{
		}

		protected override void Load()
		{
			base.Load();
			Value = PlayerPrefs.GetFloat(SaveKey, 0f);
		}

		public override void Save()
		{
			base.Save();
			PlayerPrefs.SetFloat(SaveKey, Value);
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