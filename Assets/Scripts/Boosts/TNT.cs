using UnityEngine;

namespace Boosts
{
    public class TNT : Boost
    {
        protected override void Interact()
        {
            var yIndex = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(gfxComponent.position).y);

            for (var x = 0; x < 10; x++)
            {
                
            }
            
        }
    }
}
