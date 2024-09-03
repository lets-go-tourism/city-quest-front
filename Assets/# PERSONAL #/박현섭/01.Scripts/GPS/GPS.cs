using System;
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

    public LocationInfo LocationInfo { get; private set; }

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

    private void Update()
    {
        GetLocation();
    }

    public Vector3 GetUserWorldPosition()
    {
        if (gpsTestPos)
            return testPos;
        else
            return new Vector3((float)MercatorProjection.lonToX(Longtitude), 0, (float)MercatorProjection.latToY(Latitude)) - MapReader.Instance.bounds.Center;
    }

    public double GetUserToDist(double lat, double lon)
    {
        return Distance(lat, lon, Latitude, Longtitude, 'm');
    }

    public bool GetLocation()
    {
        //statusText.text = locationService.status.ToString();

        if (!locationService.isEnabledByUser)
        {
            return false;
        }
        switch (locationService.status)
        {
            case LocationServiceStatus.Stopped:
            case LocationServiceStatus.Failed:
            case LocationServiceStatus.Initializing:
                return false;

            default:
                LocationInfo = locationService.lastData;
                Latitude = LocationInfo.latitude;
                Longtitude = LocationInfo.longitude;
                //UpdateLocationUI(info); // 임시
                return true;
        }
    }

    private void UpdateLocationUI(LocationInfo info)
    {
        //latitudeText.text = "lat: " + info.latitude.ToString();
        //longtitudeText.text = "long: " + info.longitude.ToString();
        //altitudeText.text = "alt: " + info.altitude.ToString();
    }

    public void DistanceSort() //각 함수 거리 확인후 업데이트 후 솔트해줌
    {
        List<ServerTourInfo> tourPlacesList = DataManager.instance.GetHometourPlacesList();
        LocationInfo info = DataManager.instance.GetGPSInfo();


        for (int i = 0; i < tourPlacesList.Count; i++)
        {
            double placeLatitude = Double.Parse(tourPlacesList[i].latitude);
            double placeLontitude = Double.Parse(tourPlacesList[i].longitude);

            tourPlacesList[i].distance = Distance(placeLatitude, info.latitude, placeLontitude, info.longitude, 'M').ToString();
        }

        tourPlacesList.Sort((t1, t2) =>
        {
            double distance1 = Double.Parse(t1.distance);
            double distance2 = Double.Parse(t2.distance);
            distance1.CompareTo(distance2);
            return distance1.CompareTo(distance2);
        });

        DataManager.instance.SetHometourPlaceList(tourPlacesList);
    }


    public void DistanceCheck() //관광정보 리스트들에 위도 경도와 나의 GPS 거리를 재서 업데이트함
    {
        List<ServerTourInfo> tourPlacesList = DataManager.instance.GetHometourPlacesList();
        LocationInfo info = DataManager.instance.GetGPSInfo();


        for (int i = 0; i < tourPlacesList.Count; i++)
        {
            double placeLatitude = Double.Parse(tourPlacesList[i].latitude);
            double placeLontitude = Double.Parse(tourPlacesList[i].longitude);

            tourPlacesList[i].distance = Distance(placeLatitude, info.latitude, placeLontitude, info.longitude, 'M').ToString();
        }

        //전체를한번에
    }

    public double Distance(double lat1, double lon1, double lat2, double lon2, char unit) //각 두 곳의 위도경도를 알면 unit의 거리단위로 거리를 측정하는 함수
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
        //하나하나마다계산
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
