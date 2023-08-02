using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Money
{
    public class MoneyTest : MonoBehaviour
    {
        [Button()]
        public void AddMoney() => MoneyHandler.Add(1000);
        
        [Button()]
        public void SubtractMoney() => MoneyHandler.TrySubtract(100);
        
        [Button()]
        public void Print() => print(MoneyHandler.GetMoneyCount().ToString());
    }
}
