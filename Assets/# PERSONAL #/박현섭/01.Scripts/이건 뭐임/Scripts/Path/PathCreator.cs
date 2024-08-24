using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ¸ô·ç
{
    public class PathCreator : MonoBehaviour
    {
        [HideInInspector]
        public Path path;

        public ulong nodeId;
        public void CreatePath()
        {
            path = new Path();
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var t in path.pathSections[0])
            {
                Gizmos.DrawSphere(t, 1);
            }
        }

    }
}

