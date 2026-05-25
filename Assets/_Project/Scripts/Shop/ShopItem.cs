using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Shop
{
    [RequireComponent(typeof(Button))]
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private Image _image;
        
        private Button _button;
        private ShopItemConfig _config;
        
        public void Init(ShopItemConfig config)
        {
            _config = config;
            _titleText.text = _config.Title;
            
            float imageWidth = _image.rectTransform.sizeDelta.x;
            _image.sprite = _config.Icon;
            _image.SetNativeSize();
            float originalWidth = _image.rectTransform.rect.width;
            float originalHeight = _image.rectTransform.rect.height;
            float scaleFactor = imageWidth / originalWidth;
            _image.rectTransform.sizeDelta = new Vector2(imageWidth, originalHeight * scaleFactor);
            
            UpdatePriceText();
            UpdateCountText();
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            if (G.Instance.ShopHandler.TryBuy(_config))
            {
                UpdatePriceText();
                UpdateCountText();
            }
        }

        private void UpdatePriceText()
        {
            _priceText.text = G.Instance.ShopHandler.GetItemPrice(_config) + "$";
        }
        
        private void UpdateCountText()
        {
            _countText.text = G.Instance.ShopHandler.GetItemBuyCount(_config) + "/" + _config.MaxCount;
        }
    }
}