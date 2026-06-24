using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Animations
{
    [RequireComponent(typeof(Hunter))]
    public class HunterAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Animator _animator;

        private Hunter _hunter;

        private static readonly int IsMove = Animator.StringToHash("IsMove");
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        
        private void Awake()
        {
            _hunter = GetComponent<Hunter>();
        }
        
        private void OnEnable()
        {
            transform.DOScale(1f, 0.2f).From(0f);
        }

        private void Update()
        {
            _animator.SetBool(IsMove, _hunter.IsMoving);
            _animator.SetBool(IsAttack, _hunter.IsAttacking);
            _renderer.flipX = _hunter.Direction.x < 0;
        }
    }
}