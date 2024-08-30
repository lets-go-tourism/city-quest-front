using System.Collections;
using System.Collections.Generic;
using System;
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
                UpdateLocationUI(info); // 임시
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

    public void distanceSort() //각 함수 거리 확인후 업데이트 후 솔트해줌
    {
        List<HometourPlace> tourPlacesList = DataManager.instance.GetHometourPlacesList();
        LocationInfo info = DataManager.instance.GetGPSInfo();


        for (int i = 0; i < tourPlacesList.Count; i++)
        {
            double placeLatitude = Double.Parse(tourPlacesList[i].latitude);
            double placeLontitude = Double.Parse(tourPlacesList[i].longitude);

            tourPlacesList[i].distance = distance(placeLatitude, info.latitude, placeLontitude, info.longitude, 'M').ToString();
        }

        tourPlacesList.Sort((t1, t2) =>
        {
            double distance1 = Double.Parse(t1.distance);
            double distance2 = Double.Parse(t2.distance);
            distance1.CompareTo(distance2);
            return distance1.CompareTo(distance2);
        });

        //foreach (HometourPlace tour in tourPlacesList)
        //{
        //    Debug.Log("Distance: " + tour.distance);
        //}
    }

    private double distance(double lat1, double lon1, double lat2, double lon2, char unit) //각 두 곳의 위도경도를 알면 unit의 거리단위로 거리를 측정하는 함수
    {
        if ((lat1 == lat2) && (lon1 == lon2))
        {
            return 0;
        }
        else
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else
            {
                dist = dist * 1609.344;
            }
            return (dist);
        }
    }

    private double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    private double rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }
}
