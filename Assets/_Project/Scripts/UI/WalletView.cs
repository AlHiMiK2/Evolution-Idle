using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace _Project.Scripts
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private float _amplitude = 5f;
        [SerializeField] private float _frequency = 2f;
        [SerializeField] private float _letterSpacing = 0.1f;
        [SerializeField] private Color _firstLetterColor = Color.yellow;
        [SerializeField] private bool _colorFirstLetter = true;

        private float _time;
        private double _money;
        private TMP_TextInfo _textInfo;
        private List<Vector3> _originalVertices = new List<Vector3>();
        private List<Color32> _originalColors = new List<Color32>();
        
        private void OnEnable()
        {
            _wallet.MoneyChanged += OnMoneyChanged;
        }
        
        private void OnDisable()
        {
            _wallet.MoneyChanged -= OnMoneyChanged;
        }
        
        private void OnMoneyChanged(double money, double moneyDifference)
        {
            _money = money;
            _moneyText.SetText("$" + PolyLabs.ShortScale.ParseDouble(_money, 2, 10000, true));
            _originalVertices.Clear();
            _originalColors.Clear();
            UpdateOriginalData();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            AnimateText();
        }

        private void UpdateOriginalData()
        {
            _moneyText.ForceMeshUpdate();
            _textInfo = _moneyText.textInfo;
            
            if (_textInfo == null || _textInfo.characterCount == 0)
                return;
                
            for (int i = 0; i < _textInfo.characterCount; i++)
            {
                var charInfo = _textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                    continue;
                    
                int vertexIndex = charInfo.vertexIndex;
                int materialIndex = charInfo.materialReferenceIndex;
                
                for (int j = 0; j < 4; j++)
                {
                    Vector3 originalPos = _textInfo.meshInfo[materialIndex].vertices[vertexIndex + j];
                    _originalVertices.Add(originalPos);
                    
                    Color32 originalColor = _textInfo.meshInfo[materialIndex].colors32[vertexIndex + j];
                    _originalColors.Add(originalColor);
                }
            }
            
            if (_colorFirstLetter && _textInfo.characterCount > 0)
            {
                ApplyColorToFirstLetter();
            }
        }
        
        private void ApplyColorToFirstLetter()
        {
            if (_textInfo == null || _textInfo.characterCount == 0)
                return;
                
            for (int i = 0; i < _textInfo.characterCount; i++)
            {
                var charInfo = _textInfo.characterInfo[i];
                if (charInfo.isVisible)
                {
                    int materialIndex = charInfo.materialReferenceIndex;
                    var colors = _textInfo.meshInfo[materialIndex].colors32;
                    int vertexIndex = charInfo.vertexIndex;
                    
                    for (int j = 0; j < 4; j++)
                    {
                        colors[vertexIndex + j] = _firstLetterColor;
                    }
                    
                    _textInfo.meshInfo[materialIndex].mesh.colors32 = colors;
                    _moneyText.UpdateGeometry(_textInfo.meshInfo[materialIndex].mesh, materialIndex);
                    break;
                }
            }
        }
        
        private void AnimateText()
        {
            _moneyText.ForceMeshUpdate();
            _textInfo = _moneyText.textInfo;
            
            if (_textInfo == null || _textInfo.characterCount == 0 || _originalVertices.Count == 0)
                return;
                
            int vertexIndex = 0;
            int characterIndex = 0;
            
            for (int i = 0; i < _textInfo.characterCount; i++)
            {
                var charInfo = _textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                    continue;
                    
                int materialIndex = charInfo.materialReferenceIndex;
                var vertices = _textInfo.meshInfo[materialIndex].vertices;
                var colors = _textInfo.meshInfo[materialIndex].colors32;
                
                float phaseOffset = i * _letterSpacing;
                float yOffset = Mathf.Sin(_time * _frequency + phaseOffset) * _amplitude;
                
                for (int j = 0; j < 4; j++)
                {
                    if (vertexIndex + j < _originalVertices.Count)
                    {
                        Vector3 newPos = _originalVertices[vertexIndex + j];
                        newPos.y += yOffset;
                        vertices[charInfo.vertexIndex + j] = newPos;
                        
                        if (_colorFirstLetter && characterIndex == 0 && j < 4)
                        {
                            colors[charInfo.vertexIndex + j] = _firstLetterColor;
                        }
                        else if (vertexIndex + j < _originalColors.Count)
                        {
                            colors[charInfo.vertexIndex + j] = _originalColors[vertexIndex + j];
                        }
                    }
                }
                
                vertexIndex += 4;
                characterIndex++;
            }
            
            for (int i = 0; i < _textInfo.meshInfo.Length; i++)
            {
                _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
                _textInfo.meshInfo[i].mesh.colors32 = _textInfo.meshInfo[i].colors32;
                _moneyText.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
            }
        }
        
        public void RefreshOriginalData()
        {
            UpdateOriginalData();
        }
        
        private void OnValidate()
        {
            if (_moneyText != null && Application.isPlaying)
            {
                RefreshOriginalData();
            }
        }
    }
}