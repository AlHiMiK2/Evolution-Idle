using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Animations
{
    [RequireComponent(typeof(Plant))]
    public class PlantAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _offsetY;
        [SerializeField] private float _dieScale;
        [SerializeField] private Ease _ease;
        [SerializeField] private SpriteRenderer _shadow;

        private Material _material;
        private Plant _plant;
        private bool _isHitEffect;
        
        private static readonly int HitEffectBlend = Shader.PropertyToID("_HitEffectBlend");

        private void Awake()
        {
            _plant = GetComponent<Plant>();
            _material = _renderer.material;
        }

        private void OnEnable()
        {
            transform.DOScale(1f, 0.2f).From(0f);
        }

        private void Start()
        {
            _plant.Died += OnDied;
            OnSpawned();
        }
        
        private void OnDestroy()
        {
            _plant.Died -= OnDied;
        }

        private void Update()
        {
            if (_plant.Owner && !_isHitEffect)
            {
                StartHitEffect();
            }
            else if (!_plant.Owner && _isHitEffect)
            {
                StopHitEffect();
            }
        }
        
        private void StartHitEffect()
        {
            _material
                .DOFloat(1f, HitEffectBlend, 0.6f)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.InQuad);
            _isHitEffect = true;
        }

        private void StopHitEffect()
        {
            _material.DOKill();
            _material.SetFloat(HitEffectBlend, 0f);
            _isHitEffect = false;
        }

        private void OnSpawned()
        {
            _renderer.color = Color.white;
            _renderer.transform.localPosition = Vector3.zero;
            _renderer.transform.localScale = Vector3.one;
            _shadow.DOFade(0.5f, 0);
            StopHitEffect();
        }
        
        private void OnDied()
        {
            _shadow.DOFade(0, _plant.DieDuration).SetEase(_ease);
            _renderer.transform.DOMoveY(_offsetY, _plant.DieDuration).SetEase(_ease).SetRelative(true);
            _renderer.transform.DOScale(_dieScale, _plant.DieDuration).SetEase(_ease);
            _material.DOFloat(1f, HitEffectBlend, _plant.DieDuration).SetEase(_ease);
            _renderer.DOFade(0, _plant.DieDuration * 0.5f).SetEase(_ease).SetDelay(_plant.DieDuration * 0.5f);
        }
    }
}