using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class GPS : MonoBehaviour
{
    public static GPS Instance;

    [Header("임시 현재 위치")]
    public bool gpsTestPos = false;
    public Vector3 testPos;

    [Header("Setting")]
    public bool startGPSOnStart;
    public float desiredAccuracyInMeters;
    public float updateDistanceInMeters;

    [Header("Chche")]
    private LocationService locationService;

    public float Latitude { get; private set; }
    public float Longtitude { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        locationService = Input.location;
    }

    private void Start()
    {
        if (startGPSOnStart)
        {
            StartGPS();
        }
    }

    private void Update()
    {
        UpdateMyGPS();
    }

    public Vector3 GetCurrentGPSPos()
    {
        if (gpsTestPos)
            return testPos;
        else
            return new Vector3((float)MercatorProjection.lonToX(Longtitude), 0, (float)MercatorProjection.latToY(Latitude)) - MapReader.Instance.bounds.Center;
    }

    public void StartGPS(string permissionName = null)
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            locationService.Start(desiredAccuracyInMeters, updateDistanceInMeters);
            //statusText.text = locationService.status.ToString();
        }
        else
        {
            PermissionCallbacks callbacks = new();
            callbacks.PermissionGranted += StartGPS;
            Permission.RequestUserPermission(Permission.FineLocation, callbacks);
        }
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
        //statusText.text = locationService.status.ToString();

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
                Latitude = info.latitude;
                Longtitude = info.longitude;
                altitude = info.altitude;
                UpdateLocationUI(info); // 임시
                return true;
        }
    }

    private void UpdateLocationUI(LocationInfo info)
    {
        //latitudeText.text = "lat: " + info.latitude.ToString();
        //longtitudeText.text = "long: " + info.longitude.ToString();
        //altitudeText.text = "alt: " + info.altitude.ToString();
    }
}
