using System;
using UnityEngine;
using UnityEngine.Splines;

namespace Track
{
    public class WidthData : MonoBehaviour
    {
        public float defaultWidth = 1f;

        public SplineData<float> width;

        public int Count => width.Count;
        
        private SplineContainer containerCache;
        public SplineContainer Container
        {
            get
            {
                if(containerCache == null)
                    containerCache = GetComponent<SplineContainer>();
                return containerCache;
            }
            set => containerCache = value;
        }
    }
}
