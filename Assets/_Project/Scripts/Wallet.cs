using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private int _money;

        public event UnityAction<int> MoneyChanged;

        private void Start()
        {
            MoneyChanged?.Invoke(_money);
        }

        public void AddMoney(int money)
        {
            _money += money;
            MoneyChanged?.Invoke(_money);
        }
        
        public void AddMoneyWithEffect(int money, Vector3 position)
        {
            _money += money;
            MoneyChanged?.Invoke(_money);
            G.Instance.UIHandler.CreateMoneyEffect(position, money);
        }

        public bool EnoughMoney(int money)
        {
            return _money >= money;
        }
        
        public bool TryTakeMoney(int money)
        {
            if (_money >= money)
            {
                _money -= money;
                MoneyChanged?.Invoke(_money);
                return true;
            }
            
            return false;
        }
    }
}