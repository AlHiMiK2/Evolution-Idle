using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Shop
{
    [RequireComponent(typeof(Button))]
    public class PercItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Image _image;
        [SerializeField] private Image _moneyFlagImage;
        [SerializeField] private Color _enoughMoneyColor;
        [SerializeField] private Color _notEnoughMoneyColor;
        
        private Button _button;
        private ShopItemConfig _config;
        
        public void Init(ShopItemConfig config)
        {
            _config = config;
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
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ButtonClicked);
            G.Instance.Wallet.MoneyChanged += UpdateMoneyFlag;
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
    }
}