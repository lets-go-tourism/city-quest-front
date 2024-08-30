using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class KJY_GPS : MonoBehaviour
{
    [Header("Setting")]
    public bool startGPSOnStart;
    public float desiredAccuracyInMeters;
    public float updateDistanceInMeters;

    [Header("Chche")]
    private LocationService locationService;

    [Header("LocationUI")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI latitudeText;
    [SerializeField] private TextMeshProUGUI longtitudeText;
    [SerializeField] private TextMeshProUGUI altitudeText;

    private void Awake()
    {
        locationService = Input.location;
    }

    private void Start()
    {
        if (startGPSOnStart)
        {
            StartGPS();
        }
    }

    public void StartGPS(string permissionName = null)
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            locationService.Start(desiredAccuracyInMeters, updateDistanceInMeters);
            statusText.text = locationService.status.ToString();
        }
        else
        {
            PermissionCallbacks callbacks = new();
            callbacks.PermissionGranted += StartGPS;
            Permission.RequestUserPermission(Permission.FineLocation, callbacks);
        }
    }
        public void StopGPS()
    {
        locationService.Stop();
    }

    public void UpdateMyGPS()
    {
        GetLocation(out var status, out var latitude, out var longitude, out var altitude);
    }

    public bool GetLocation(out LocationServiceStatus status, out float latitude, out float longtitude, out float altitude)
    {
        latitude = 0;
        longtitude = 0;
        altitude = 0;
        status = locationService.status;
        statusText.text = locationService.status.ToString();

        if (!locationService.isEnabledByUser)
        {
            return false;
        }
        switch (status)
        {
            case LocationServiceStatus.Stopped:
            case LocationServiceStatus.Failed:
            case LocationServiceStatus.Initializing:
                return false;

            default:
                LocationInfo info = locationService.lastData;
                latitude = info.latitude;
                longtitude = info.longitude;
                altitude = info.altitude;
                UpdateLocationUI(info); // юс╫ц
                DataManager.instance.SetGPSInfo(info);
                return true;
        }
    }

    private void UpdateLocationUI(LocationInfo info)
    {
        latitudeText.text = "lat: " + info.latitude.ToString();
        longtitudeText.text = "long: " + info.longitude.ToString();
        altitudeText.text = "alt: " + info.altitude.ToString();
    }
}
