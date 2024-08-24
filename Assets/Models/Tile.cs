using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Helpers;
using Assets.Models;
using UnityEngine;

namespace Assets
{
    public class Tile : MonoBehaviour
    {
        public Rect Rect;
        Dictionary<Vector3, BuildingHolder> BuildingDictionary { get; set; }

        public Tile()
        {
            BuildingDictionary = new Dictionary<Vector3, BuildingHolder>();
        }

        public IEnumerator CreateTile(World w, Vector2 realPos, Vector2 worldCenter, int zoom)
        {
            var tilename = realPos.x + "_" + realPos.y;
            var tileurl = realPos.x + "/" + realPos.y;
            var url = "http://vector.mapzen.com/osm/water,earth,buildings,roads,landuse/" + zoom + "/";

            JSONObject mapData;
            if (File.Exists(tilename))
            {
                var r = new StreamReader(tilename, Encoding.Default);
                mapData = new JSONObject(r.ReadToEnd());
            }
            else
            {
                var www = new WWW(url + tileurl + ".json");
                yield return www;

                var sr = File.CreateText(tilename);
                sr.Write(www.text);
                sr.Close();

                mapData = new JSONObject(www.text);
            }
            Rect = GM.TileBounds(realPos, zoom);

            CreateBuildings(mapData["buildings"]);
            CreateRoads(mapData["roads"]);
        }

        private void CreateBuildings(JSONObject mapData)
        {
            foreach (var geo in mapData["features"].list.Where(x => x["geometry"]["type"].str == "Polygon"))
            {
                var l = new List<Vector3>();
                for (int i = 0; i < geo["geometry"]["coordinates"][0].list.Count - 1; i++)
                {
                    var c = geo["geometry"]["coordinates"][0].list[i];
                    var bm = GM.LatLonToMeters(c[1].f, c[0].f);
                    var pm = new Vector2(bm.x - Rect.center.x, bm.y - Rect.center.y);
                    l.Add(pm.ToVector3xz());
                }

                try
                {
                    var center = l.Aggregate((acc, cur) => acc + cur) / l.Count;
                    if (!BuildingDictionary.ContainsKey(center))
                    {
                        var bh = new BuildingHolder(center, l);
                        for (int i = 0; i < l.Count; i++)
                        {
                            l[i] = l[i] - bh.Center;
                        }
                        BuildingDictionary.Add(center, bh);

                        var m = bh.CreateModel();
                        m.name = "building";
                        m.transform.parent = this.transform;
                        m.transform.localPosition = center;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

        private void CreateRoads(JSONObject mapData)
        {
            foreach (var geo in mapData["features"].list)
            {
                var l = new List<Vector3>();

                for (int i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                {
                    var c = geo["geometry"]["coordinates"][i];
                    var bm = GM.LatLonToMeters(c[1].f, c[0].f);
                    var pm = new Vector2(bm.x - Rect.center.x, bm.y - Rect.center.y);
                    l.Add(pm.ToVector3xz());
                }

                var m = new GameObject("road").AddComponent<RoadPolygon>();
                m.transform.parent = this.transform;
                try
                {
                    m.Initialize(geo["properties"]["id"].str, this, l, geo["properties"]["kind"].str);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }
    }
}
