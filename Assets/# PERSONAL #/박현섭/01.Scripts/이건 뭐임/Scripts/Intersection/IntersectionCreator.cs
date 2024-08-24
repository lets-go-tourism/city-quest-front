using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ¸ô·ç
{
    public class IntersectionCreator : MonoBehaviour
    {
        [HideInInspector]
        public Intersection intersection;

        public void CreateIntersection()
        {
            intersection = new Intersection();
        }
    }
}

