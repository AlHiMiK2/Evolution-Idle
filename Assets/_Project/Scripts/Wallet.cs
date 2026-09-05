using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private double _money;

        public event UnityAction<double, double> MoneyChanged;

        private void Start()
        {
            MoneyChanged?.Invoke(_money, 0);
        }

        public void AddMoney(double money)
        {
            _money += money;
            MoneyChanged?.Invoke(_money, money);
        }
        
        public void AddMoneyWithEffect(double money, Vector3 position)
        {
            _money += money;
            MoneyChanged?.Invoke(_money, money);
            G.Instance.UIHandler.CreateMoneyEffect(position, money);
        }

        public bool EnoughMoney(double money)
        {
            return _money >= money;
        }
        
        public bool TryTakeMoney(double money)
        {
            if (_money >= money)
            {
                _money -= money;
                MoneyChanged?.Invoke(_money, -money);
                return true;
            }
            
            return false;
        }
    }
}