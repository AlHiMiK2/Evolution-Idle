using _Project.Scripts.States;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Animations
{
    [RequireComponent(typeof(Entity))]
    public class AnimalAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _offsetY;
        [SerializeField] private float _dieScale;
        [SerializeField] private Ease _ease;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _shadow;
        [SerializeField] private EntityData _data;
        [SerializeField] private DeadState _deadState;

        private Material _material;
        private bool _isHitEffect;
        private bool _isDead;
        
        private static readonly int IsMove = Animator.StringToHash("IsMove");
        private static readonly int IsHit = Animator.StringToHash("IsHit");
        private static readonly int IsDie = Animator.StringToHash("IsDie");
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        private static readonly int HitEffectBlend = Shader.PropertyToID("_HitEffectBlend");
        private static readonly int ShineLocation = Shader.PropertyToID("_ShineLocation");
        
        private void Awake()
        {
            _material = _renderer.material;
        }
        
        private void OnEnable()
        {
            _deadState.Died += OnDied;
            OnSpawned();
        }
        
        private void OnDisable()
        {
            _deadState.Died -= OnDied;
        }

        private void Update()
        {
            if (_data.KillReward > 0)
            {
                _material.SetFloat(ShineLocation, Mathf.Sin(Time.time * 2f) / 2 + 0.5f);
                _material.EnableKeyword("SHINE_ON");
            }
            else
            {
                _material.DisableKeyword("SHINE_ON");
            }
            if(_data.IsDead) return;
            _animator.SetBool(IsMove, _data.Direction.sqrMagnitude > 0f);
            _animator.SetBool(IsAttack, _data.IsAttacking);
            _animator.SetBool(IsHit, _data.Owner);
            _renderer.flipX = _data.Direction.x < 0;

            if (_data.Owner && !_isHitEffect)
            {
                StartHitEffect();
            }
            else if (!_data.Owner && _isHitEffect)
            {
                StopHitEffect();
            }
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
        
        private void OnSpawned()
        {
            _renderer.color = Color.white;
            _renderer.transform.localPosition = Vector3.zero;
            _renderer.transform.localScale = Vector3.one;
            _animator.SetBool(IsDie, false);
            transform.DOScale(1f, 0.2f).From(0f);
            _shadow.DOFade(0.5f, 0);
            StopHitEffect();
        }
        
        private void OnDied()
        {
            _animator.SetBool(IsDie, true);
            _shadow.DOFade(0, _data.DieDuration).SetEase(_ease);
            _renderer.transform.DOMoveY(_offsetY, _data.DieDuration).SetEase(_ease).SetRelative(true);
            _renderer.transform.DOScale(_dieScale, _data.DieDuration).SetEase(_ease);
            _material.DOFloat(1f, HitEffectBlend, _data.DieDuration).SetEase(_ease);
            _renderer.DOFade(0, _data.DieDuration * 0.5f).SetEase(_ease).SetDelay(_data.DieDuration * 0.5f);
        }
    }
}