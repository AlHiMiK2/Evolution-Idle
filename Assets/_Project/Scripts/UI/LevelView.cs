using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _progressImage;

        private void OnEnable()
        {
            G.Instance.LevelHandler.LevelChanged += OnLevelChanged;
            G.Instance.LevelHandler.ProgressChanged += OnProgressChanged;
        }

        private void OnDisable()
        {
            G.Instance.LevelHandler.LevelChanged -= OnLevelChanged;
            G.Instance.LevelHandler.ProgressChanged -= OnProgressChanged;
        }

        private void OnLevelChanged(int level)
        {
            _levelText.SetText("Level {0}", level + 1);
        }

        private void OnProgressChanged(float progress)
        {
            _progressImage.fillAmount = progress;
        }
    }
}