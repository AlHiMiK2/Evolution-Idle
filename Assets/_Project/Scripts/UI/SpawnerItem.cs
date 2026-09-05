using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Shop
{
    [RequireComponent(typeof(Button))]
    public class SpawnerItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private Image _image;
        [SerializeField] private Image _moneyFlagImage;
        [SerializeField] private Image _spawnProgress;
        [SerializeField] private Color _enoughMoneyColor;
        [SerializeField] private Color _notEnoughMoneyColor;
        
        private Button _button;
        private ShopItemConfig _config;
        private EntitySpawner _spawner;
        private bool _isInited = false;
        
        public void Init(ShopItemConfig config, EntitySpawner spawner)
        {
            _config = config;
            _spawner = spawner;
            _titleText.SetText(_config.Title);

            float maxWidth = _image.rectTransform.rect.width;
            float maxHeight = _image.rectTransform.rect.height;

            _image.sprite = _config.Icon;
            _image.SetNativeSize();

            float originalWidth = _image.rectTransform.rect.width;
            float originalHeight = _image.rectTransform.rect.height;

            float widthScale = maxWidth / originalWidth;
            float heightScale = maxHeight / originalHeight;

            float scaleFactor = Mathf.Min(widthScale, heightScale);

            _image.rectTransform.sizeDelta = new Vector2(originalWidth * scaleFactor, originalHeight * scaleFactor);
            
            UpdatePriceText();
            UpdateMoneyFlag();
            _isInited = true;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (_isInited)
            {
                UpdateMoneyFlag();
            }
            _button.onClick.AddListener(ButtonClicked);
            G.Instance.Wallet.MoneyChanged += UpdateMoneyFlag;
        }

        private void Update()
        {
            UpdateSpawnProgress();
            UpdateCountText();
        }

        private void OnDisable()
        {
            G.Instance.Wallet.MoneyChanged -= UpdateMoneyFlag;
        }
        
        private void ButtonClicked()
        {
            if (G.Instance.ShopHandler.TryBuy(_config))
            {
                UpdatePriceText();
            }
        }

        private void UpdatePriceText()
        {
            if (_config.IsSingle && G.Instance.ShopHandler.GetItemBuyCount(_config) > 0)
            {
                _priceText.SetText("Selled!");
            }
            else
            {
                double price = G.Instance.ShopHandler.GetItemPrice(_config);
                string priceText = PolyLabs.ShortScale.ParseDouble(price, 1, 10000, true);
                _priceText.SetText(priceText + "$");
            }
        }
        
        private int _cachedLive;
        private int _cachedSpawn;
        
        private void UpdateCountText()
        {
            if (_spawner.LiveCount != _cachedLive || _spawner.SpawnCount != _cachedSpawn)
            {
                _cachedLive = _spawner.LiveCount;
                _cachedSpawn = _spawner.SpawnCount;
                _countText.SetText("{0}/{1}", _cachedLive, _cachedSpawn);
            }
        }

        private void UpdateMoneyFlag(double money = 0, double moneyDifference = 0)
        {
            if (G.Instance.Wallet.EnoughMoney(G.Instance.ShopHandler.GetItemPrice(_config)))
            {
                _moneyFlagImage.color = _enoughMoneyColor;
            }
            else
            {
                _moneyFlagImage.color = _notEnoughMoneyColor;
            }
        }

        private void UpdateSpawnProgress()
        {
            _spawnProgress.fillAmount = _spawner.SpawnProgress;
        }
    }
}