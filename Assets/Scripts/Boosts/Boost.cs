using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Boosts
{
    public abstract class Boost : MonoBehaviour
    {
        // 4, 5
        [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private Vector3 spawnPositionUI;

        [SerializeField] private Button interact;
        
        int _boostLayerId = 10;

        [SerializeField] protected RectTransform gfxComponent;

        private void Start()
        {
            spawnPositionUI = Camera.main.WorldToScreenPoint(spawnPosition);
            _boostLayerId = LayerMask.NameToLayer("boost");
            interact.onClick.AddListener(Interact);
        }

        protected abstract void Interact();

        public void SpawnBoost()
        {
            gfxComponent.gameObject.SetActive(true);
            gfxComponent.position = spawnPositionUI;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(pos, Vector2.zero);
            
            if (hit.collider && hit.collider.gameObject.layer == _boostLayerId)
            {
                StartCoroutine(MovingBlock());
            }
        }

        private IEnumerator MovingBlock()
        {
            while (Input.GetMouseButton(0))
            {
                var fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var intPos = Board.Vector3ToVector2Int(fingerPos);

                var newPos = new Vector3 {x = intPos.x, y = intPos.y};

                gfxComponent.position = newPos;
                
                yield return null;
            }
        }
    }
}
