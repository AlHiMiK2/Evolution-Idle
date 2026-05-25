using UnityEngine;

namespace _Project.Scripts.UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private MoneyEffect _moneyEffect;
        
        public void CreateMoneyEffect(Vector2 position, int money)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);
            var instance = Instantiate(_moneyEffect, screenPosition, Quaternion.identity);
            instance.transform.SetParent(transform);
            instance.Init(money);
        }
    }
}