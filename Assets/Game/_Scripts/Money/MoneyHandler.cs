using System;
using _Scripts.Utils.SavableValues;
using UnityEngine;

namespace _Scripts.Money
{
	public static class MoneyHandler
	{
		public static event Action<float, Vector3> OnMoneyAdd;
		public static event Action<float> OnMoneySubtracted;
		
		private static readonly IntDataValueSavable _moneyAmount = new("player_money_amount");

		public static bool Add(int addedValue, Vector3 screenPosition = default)
		{
			if (addedValue < 0)
			{
				Debug.LogWarning("Try Add Negative Money Amount");
				return false;
			}

			_moneyAmount.Value += addedValue;
			_moneyAmount.Save();
			
			OnMoneyAdd?.Invoke(addedValue, screenPosition);
			return true;
		}

		public static bool TrySubtract(int subtractValue)
		{
			if (subtractValue < 0)
			{
				Debug.LogWarning("Try Subtract Negative Money Amount");
				return false;
			}

			if (_moneyAmount.Value < subtractValue)
			{
				Debug.LogWarning("Not Enough Money");
				return false;
			}
			
			_moneyAmount.Value -= subtractValue;
			_moneyAmount.Save();
			OnMoneySubtracted?.Invoke(subtractValue);

			return true;
		}
		
		public static int GetMoneyCount() => _moneyAmount.Value;
	}
}