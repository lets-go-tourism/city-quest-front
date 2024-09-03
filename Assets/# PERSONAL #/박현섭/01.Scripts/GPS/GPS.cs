using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class GPS : MonoBehaviour
{
    public static GPS Instance;

    [Header("�ӽ� ���� ��ġ")]
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
                //UpdateLocationUI(info); // �ӽ�
                return true;
        }
    }

    private void UpdateLocationUI(LocationInfo info)
    {
        //latitudeText.text = "lat: " + info.latitude.ToString();
        //longtitudeText.text = "long: " + info.longitude.ToString();
        //altitudeText.text = "alt: " + info.altitude.ToString();
    }

    public void DistanceSort() //�� �Լ� �Ÿ� Ȯ���� ������Ʈ �� ��Ʈ����
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


    public void DistanceCheck() //�������� ����Ʈ�鿡 ���� �浵�� ���� GPS �Ÿ��� �缭 ������Ʈ��
    {
        List<ServerTourInfo> tourPlacesList = DataManager.instance.GetHometourPlacesList();
        LocationInfo info = DataManager.instance.GetGPSInfo();


        for (int i = 0; i < tourPlacesList.Count; i++)
        {
            double placeLatitude = Double.Parse(tourPlacesList[i].latitude);
            double placeLontitude = Double.Parse(tourPlacesList[i].longitude);

            tourPlacesList[i].distance = Distance(placeLatitude, info.latitude, placeLontitude, info.longitude, 'M').ToString();
        }

        //��ü���ѹ���
    }

    public double Distance(double lat1, double lon1, double lat2, double lon2, char unit) //�� �� ���� �����浵�� �˸� unit�� �Ÿ������� �Ÿ��� �����ϴ� �Լ�
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
        //�ϳ��ϳ����ٰ��
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
