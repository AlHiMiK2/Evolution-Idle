using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class MoneyEffect : MonoBehaviour
    {
        [SerializeField] private float _offsetY;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        [SerializeField] private TMP_Text _moneyText;
        
        public void Init(int money)
        {
            _moneyText.text = $"+{money}$";        
        }

        private void Start()
        {
            _moneyText.DOFade(1, _duration).From(0);
            transform.DOMoveY(_offsetY, _duration).SetEase(_ease).SetRelative(true).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}