using UnityEngine;

[RequireComponent(typeof(MapReader))]
abstract class InfrastructureBehaviour : MonoBehaviour
{
    protected MapReader map;

    void Awake()
    {
        map  = GetComponent<MapReader>();
    }
    protected Vector3 GetCentre(OsmWay way)
    {
        Vector3 total = Vector3.zero;

        if (way.NodeIDs.Count == 0)
        {
            return total;
        }

        foreach (var id in way.NodeIDs)
        {
            total += map.nodes[id];
        }

        return total / way.NodeIDs.Count;
    }
}