using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project002
{
    [System.Serializable]
    public class PlayerData 
    {
        public int currentLevel;
        public float[] playerPos;

        public PlayerData(int _currentLevel, float[] _playerPos) 
        {
            currentLevel = _currentLevel;
            playerPos = _playerPos;
        }

    }
}
