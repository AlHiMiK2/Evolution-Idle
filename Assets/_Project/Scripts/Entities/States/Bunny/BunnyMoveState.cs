using UnityEngine;

namespace _Project.Scripts.States
{
    public class BunnyMoveState : State
    {
        [SerializeField] private Entity _self; 
        [SerializeField] private BunnyData _data; 
        
        private void Update()
        {
            _data.Direction = (_data.TargetPosition - _self.transform.position).normalized;
            float speed = _data.Speed * _data.SpeedMultiplier;
            _self.transform.Translate(_data.Direction * (speed * Time.deltaTime), Space.World);
        }
    }
}