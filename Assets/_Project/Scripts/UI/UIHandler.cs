using DamageNumbersPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private DamageNumber _moneyEffect;

        private RectTransform _rect;
        
        private void Start()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void CreateMoneyEffect(Vector3 position, double money)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);
            _moneyEffect.Spawn(_rect, screenPosition / _rect.localScale, (float)money);
        }
    }
}