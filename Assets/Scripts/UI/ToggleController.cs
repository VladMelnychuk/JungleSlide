using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour 
{
	public  bool isOn;

	public Color onColorBg;
	public Color offColorBg;

	public Image toggleBgImage;
	public RectTransform toggle;

	public GameObject handle;
	private RectTransform _handleTransform;

	private float _handleSize;
	private float _onPosX;
	private float _offPosX;

	public float handleOffset;

	public GameObject onIcon;
	public GameObject offIcon;


	public float speed;
	private static float _t;

	private bool _switching;


	private void Awake()
	{
		_handleTransform = handle.GetComponent<RectTransform>();
		RectTransform handleRect = handle.GetComponent<RectTransform>();
		_handleSize = handleRect.sizeDelta.x;
		float toggleSizeX = toggle.sizeDelta.x;
		_onPosX = (toggleSizeX / 2) - (_handleSize/2) - handleOffset;
		_offPosX = _onPosX * -1;

	}


	void Start()
	{
		if(isOn)
		{
			toggleBgImage.color = onColorBg;
			_handleTransform.localPosition = new Vector3(_onPosX, 0f, 0f);
			onIcon.gameObject.SetActive(true);
			offIcon.gameObject.SetActive(false);
		}
		else
		{
			toggleBgImage.color = offColorBg;
			_handleTransform.localPosition = new Vector3(_offPosX, 0f, 0f);
			onIcon.gameObject.SetActive(false);
			offIcon.gameObject.SetActive(true);
		}
	}

	private void Update()
	{

		if(_switching)
		{
			Toggle(isOn);
		}
	}

	private void DoYourStaff()
	{
		Debug.Log(isOn);
	}
	
	private void Toggle(bool toggleStatus)
	{
		if(!onIcon.activeSelf || !offIcon.activeSelf)
		{
			onIcon.SetActive(true);
			offIcon.SetActive(true);
		}
		
		if(toggleStatus)
		{
			toggleBgImage.color = SmoothColor(onColorBg, offColorBg);
			Transparency (onIcon, 1f, 0f);
			Transparency (offIcon, 0f, 1f);
			_handleTransform.localPosition = SmoothMove(handle, _onPosX, _offPosX);
		}
		else 
		{
			toggleBgImage.color = SmoothColor(offColorBg, onColorBg);
			Transparency (onIcon, 0f, 1f);
			Transparency (offIcon, 1f, 0f);
			_handleTransform.localPosition = SmoothMove(handle, _offPosX, _onPosX);
		}
			
	}


	private Vector3 SmoothMove(GameObject toggleHandle, float startPosX, float endPosX)
	{
		var position = new Vector3 (Mathf.Lerp(startPosX, endPosX, _t += speed * Time.deltaTime), 0f, 0f);
		StopSwitching();
		return position;
	}

	private Color SmoothColor(Color startCol, Color endCol)
	{
		var resultCol = Color.Lerp(startCol, endCol, _t += speed * Time.deltaTime);
		return resultCol;
	}

	private CanvasGroup Transparency (GameObject alphaObj, float startAlpha, float endAlpha)
	{
		var alphaVal = alphaObj.gameObject.GetComponent<CanvasGroup>();
		alphaVal.alpha = Mathf.Lerp(startAlpha, endAlpha, _t += speed * Time.deltaTime);
		return alphaVal;
	}

	private void StopSwitching()
	{
		if (!(_t > 1.0f)) return;
		_switching = false;

		_t = 0.0f;
		switch(isOn)
		{
			case true:
				isOn = false;
				DoYourStaff();
				break;

			case false:
				isOn = true;
				DoYourStaff();
				break;
		}
	}

}
