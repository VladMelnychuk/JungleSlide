using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Boosts
{ 
    public class TNT : Boost
    {
        //Text num_tnt;

        protected override void Interact()
        {
            Text num_tnt;
            num_tnt = GameObject.Find("num_block").GetComponent<Text>();
            var num_int = int.Parse(num_tnt.text);
            if (num_int == 0)
            {
                print("Open game shop");
            }
            else
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
}
