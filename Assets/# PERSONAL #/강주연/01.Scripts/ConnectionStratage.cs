using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionStratage : MonoBehaviour
{
    public interface ConnectionStratege
    {
        public void CreateJson();
        public void OnGetRequest(string jsonData);
        public void Complete(DownloadHandler result);
    }
}
