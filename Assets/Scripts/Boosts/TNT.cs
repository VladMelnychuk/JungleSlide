using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Boosts
{
    public class TNT : Boost
    {

        [SerializeField] public int pulsingRow = 0;

        private List<Transform> _pulsingBlocks = new List<Transform>();
        
        void Start()
        {
        }

        void Update()
        {
        }

        public override void ApplyBoost()
        {
            BoostUIComponent.IsBoostActive = true;
        }

        public override void Interact(Vector3 pos)
        {
            var newRow = Mathf.RoundToInt(pos.y);

            if (newRow != pulsingRow)
            {
                StopAnimations();
                AddAnimations(newRow);
            }
            else
            {
                
            }
        }

        private void AddAnimations(int newRow)
        {
            for (var i = 0; i < 8; i++)
            {
                if (Board._grid[newRow, i] == null) continue;
                var blockTransform = Board._grid[newRow, i].transform;
                blockTransform.DOScale(0.7f, 0.4f).SetLoops(-1, LoopType.Yoyo);
                _pulsingBlocks.Add(blockTransform);
            }

            pulsingRow = newRow;
        }

        private void StopAnimations()
        {
            foreach (var blockTransform in _pulsingBlocks)
            {
                DOTween.Kill (blockTransform);
            }
            _pulsingBlocks.Clear();
        }
    }
}