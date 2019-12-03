using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LevelCreator : MonoBehaviour
{
    private Renderer _boardRenderer;
    private Texture _texture;

    [SerializeField] private LayerMask boardMask;

    [SerializeField] private GameObject boardGameObject;

    [SerializeField] private Button waterButton;
    [SerializeField] private Button earthButton;
    [SerializeField] private Button airButton;

    private Color _blockColor = Color.cyan;
    Vector2Int _textureSize = new Vector2Int(8, 10);

    private void Start()
    {
        waterButton.onClick.AddListener(Water);
        earthButton.onClick.AddListener(Earth);
        airButton.onClick.AddListener(Air);

        _boardRenderer = boardGameObject.GetComponent<Renderer>();

        _texture = new Texture2D(_textureSize.x, _textureSize.y) {filterMode = FilterMode.Point};

        _boardRenderer.material.mainTexture = _texture;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, boardMask))
            {
                var tex = _boardRenderer.material.mainTexture as Texture2D;
                print(tex.width + " " + tex.height);
                var pixelUV = hit.textureCoord;
                pixelUV.x *= tex.width;
                pixelUV.y *= tex.height;
                tex.SetPixel((int) pixelUV.x, (int) pixelUV.y, _blockColor);
                tex.Apply();
            }
        }
    }

    #region Colors

    private void Water()
    {
        _blockColor = Color.blue;
    }

    private void Earth()
    {
        _blockColor = Color.red;
    }

    private void Air()
    {
        _blockColor = Color.cyan;
    }

    #endregion

    private void SaveLevel()
    {
        dynamic product = new JObject();
        
        var tex = _boardRenderer.material.mainTexture as Texture2D;
        for (int xIndex = 0; xIndex < _textureSize.x; xIndex++)
        {
            for (int yIndex = 0; yIndex < _textureSize.y; yIndex++)
            {
                
                product.
            }
        }
    }

    private string getBlockName()
    {
        if (_blockColor == Color.blue) return "water";
        if (_blockColor == Color.red) return "earth";
        if (_blockColor == Color.cyan) return "air";
        return "";
    }
    
}