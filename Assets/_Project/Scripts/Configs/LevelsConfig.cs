using System;
using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "New Levels CFG", menuName = "Create Levels CFG", order = 0)]
    public class LevelsConfig : ScriptableObject
    {
        [Serializable]
        public struct Level
        {
            public double NeedMoney;
        }

        public Level[] Levels;
    }
}