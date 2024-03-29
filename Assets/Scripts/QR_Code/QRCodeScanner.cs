using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;

public class QRCodeScanner : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageBackground;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;
    [SerializeField]
    private TextMeshProUGUI _textOut;
    [SerializeField]
    private RectTransform _scanZone;

    [SerializeField]
    private Dictionary<string, GameObject> _qrCodeToObjectMap;

    
    
    private bool _isCamAvaible;
    private WebCamTexture _cameraTexture;
    void Start()
    {
        SetUpCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();
    }

    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            _isCamAvaible = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
                break;
            }
        }
        _cameraTexture.Play();
        _rawImageBackground.texture = _cameraTexture;
        _isCamAvaible = true;
    }

    private void UpdateCameraRender()
    {
        if (_isCamAvaible == false)
        {
            return;
        }
        float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;

        int orientation = _cameraTexture.videoRotationAngle;
        orientation = orientation * 3;
        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);

    }
    public void OnClickScan()
    {
        Scan();
    }
    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
            if (result != null)
            {
                _textOut.text = result.Text;

                // Assuming the QR code is in the center of the view
                Vector3 screenCenter = new Vector3(_cameraTexture.width / 2, _cameraTexture.height / 2, 0);
                DisplayObjectAtScreenPosition(result.Text, screenCenter);
            }
            else
            {
                _textOut.text = "Failed to Read QR CODE";
            }
        }
        catch
        {
            _textOut.text = "FAILED IN TRY";
        }
    }
    private void DisplayObjectAtScreenPosition(string qrResult, Vector3 screenPosition)
    {
        if (_qrCodeToObjectMap.TryGetValue(qrResult, out GameObject objectPrefab))
        {
            // Convert the screen position to a world position
            // Adjust the Z value (depth) as needed based on your camera setup and desired object location
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 3.0f));

            // Instantiate the object at the world position
            Instantiate(objectPrefab, worldPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("QR Code does not correspond to a known object.");
        }
    }
    



}
