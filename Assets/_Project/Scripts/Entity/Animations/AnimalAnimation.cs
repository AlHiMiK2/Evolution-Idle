using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Animations
{
    [RequireComponent(typeof(Animal))]
    public class AnimalAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _offsetY;
        [SerializeField] private float _dieScale;
        [SerializeField] private Ease _ease;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _shadow;

        private Animal _animal;
        private Material _material;
        private bool _isHitEffect;

        private static readonly int IsMove = Animator.StringToHash("IsMove");
        private static readonly int IsEat = Animator.StringToHash("IsEat");
        private static readonly int IsDie = Animator.StringToHash("IsDie");
        private static readonly int HitEffectBlend = Shader.PropertyToID("_HitEffectBlend");
        private static readonly int InnerOutlineAlpha = Shader.PropertyToID("_InnerOutlineAlpha");
        
        private void Awake()
        {
            _animal = GetComponent<Animal>();
            _material = _renderer.material;
        }
        
        private void OnEnable()
        {
            transform.DOScale(1f, 0.2f).From(0f);
        }

        private void Start()
        {
            _animal.Died += OnDied;
            _animal.Respawned += OnRespawned;
        }
        
        private void OnDestroy()
        {
            _animal.Died -= OnDied;
            _animal.Respawned -= OnRespawned;
        }

        private void Update()
        {
            _animator.SetBool(IsMove, _animal.IsMoving);
            _animator.SetBool(IsEat, _animal.IsAttacking || _animal.IsAttacked);
            _renderer.flipX = _animal.Direction.x < 0;

            if (_animal.IsAttacked && !_isHitEffect)
            {
                StartHitEffect();
            }
            else if (!_animal.IsAttacked && _isHitEffect)
            {
                StopHitEffect();
            }

            _material.SetFloat(InnerOutlineAlpha, _animal.KillCount / 5f);
        }

        private void StartHitEffect()
        {
            _material
                .DOFloat(1f, HitEffectBlend, 0.3f)
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
        
        private void OnRespawned()
        {
            _renderer.color = Color.white;
            _renderer.transform.localPosition = Vector3.zero;
            _renderer.transform.localScale = Vector3.one;
            _animator.SetBool(IsDie, false);
            _shadow.DOFade(0.5f, 0);
            StopHitEffect();
        }

        private void OnDied()
        {
            _animator.SetBool(IsDie, true);
            _shadow.DOFade(0, _animal.DieDelay).SetEase(_ease);
            _renderer.transform.DOMoveY(_offsetY, _animal.DieDelay).SetEase(_ease).SetRelative(true);
            _renderer.transform.DOScale(_dieScale, _animal.DieDelay).SetEase(_ease);
            _material.DOFloat(1f, HitEffectBlend, _animal.DieDelay).SetEase(_ease);
            _renderer.DOFade(0, _animal.DieDelay * 0.5f).SetEase(_ease).SetDelay(_animal.DieDelay * 0.5f);
        }
    }
}