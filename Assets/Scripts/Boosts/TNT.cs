using System.Collections.Generic;
using UnityEngine;

namespace Boosts
{
    public class TNT : Boost
    {
        protected override void Interact()
        {
            var yIndex = Mathf.RoundToInt(gfxComponent.position.y);

            var removedBlocks = new List<Block>();

            for (var x = 0; x < 8;)
            {
                var block = Board._grid[x, yIndex];
                if (block == null)
                {
                    x += 1;
                    continue;
                }
                x += block.size;
                // TODO animate
                removedBlocks.Add(block);
            }
            
            gameBoard.ApplyBoost(removedBlocks);
            
            gfxComponent.gameObject.SetActive(false);
            uiComponent.gameObject.SetActive(false);
        }
    }
}
