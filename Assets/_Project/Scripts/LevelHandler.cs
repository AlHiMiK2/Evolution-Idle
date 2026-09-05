using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts
{
    public class LevelHandler : MonoBehaviour
    {
        [SerializeField] private LevelsConfig _cfg;
        
        private int _currentLevel;
        private float _levelUpProgress;

        public event UnityAction<int> LevelChanged;
        public event UnityAction<float> ProgressChanged; 
        
        private void OnEnable()
        {
            G.Instance.Wallet.MoneyChanged += OnMoneyChanged;
        }

        private void OnDisable()
        {
            G.Instance.Wallet.MoneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged(double money, double moneyDifference)
        {
            if (moneyDifference > 0)
            {
                _levelUpProgress += (float)(moneyDifference / _cfg.Levels[_currentLevel].NeedMoney);
                
                if (_levelUpProgress >= 1f)
                {
                    _currentLevel++;
                    _levelUpProgress = 0;
                    LevelChanged?.Invoke(_currentLevel);
                }
                
                ProgressChanged?.Invoke(_levelUpProgress);
            }
        }
    }
}