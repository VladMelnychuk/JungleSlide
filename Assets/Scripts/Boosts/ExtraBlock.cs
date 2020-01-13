using System;
using DG.Tweening;
using UnityEngine;

namespace Boosts
{
    public class ExtraBlock : Boost
    {
        [SerializeField] private GameObject boostBlockPrefab;

        private void Start()
        {
            gameBoard.objectpool.AddObjectPool(BoostTag, boostBlockPrefab, gameBoard.blocksHolder, 5);
        }

        protected override void Interact()
        {
            var pos = Board.Vector3ToVector2Int(gfxComponent.position);

            if (Board._grid[pos.x, pos.y] != null)
            {
                gfxComponent.DOShakePosition(AnimationsInfo.BlockAnimationDuration, .5f, 5);
                return;
            }
            
            var extraBlock = gameBoard.objectpool[BoostTag].Spawn(spawnPosition, Quaternion.identity);

            extraBlock.transform.position = gfxComponent.position;
            var extraBlockComponent = extraBlock.GetComponent<Block>();
            extraBlockComponent.gridPosition = pos;
            gameBoard.ApplyBoost(extraBlockComponent);
            
            gfxComponent.gameObject.SetActive(false);
            uiComponent.gameObject.SetActive(false);
        }

    }
}
