using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace UnityStandardAssets.Utility
{
    public struct RoutePoint
    {
        public Vector3 position;
        public Vector3 direction;


        public RoutePoint(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }

    [ExecuteInEditMode]
    public class WaypointCircuit : MonoBehaviour
    {
        public WaypointList waypointList = new WaypointList();
        public float nfSphereSize = 0.4F;
        public bool autoSetChildren = false;
        public bool autoRename = false;

        [SerializeField] private bool smoothRoute = true;
        private int numPoints;
        private Vector3[] points;
        private float[] distances;

        public float editorVisualisationSubsteps = 100;
        public float Length { get; private set; }

        public Transform[] Waypoints
        {
            get { return waypointList.items; }
        }

        //this being here will save GC allocs
        private int p0n;
        private int p1n;
        private int p2n;
        private int p3n;

        private float i;
        private Vector3 P0;
        private Vector3 P1;
        private Vector3 P2;
        private Vector3 P3;

        // Use this for initialization
        private void Awake()
        {
            waypointList.circuit = this;
            if (Waypoints.Length > 1)
            {
                CachePositionsAndDistances();
            }
            numPoints = Waypoints.Length;
        }

        private void Update()
        {
            if (autoRename)
            {
                autoRename = false;
                AutoRename();
            }

            if (autoSetChildren)
            {
                autoSetChildren = false;
                AutoSetChildren();
            }
        }

        void AutoSetChildren()
        {

            var children = new Transform[transform.childCount];
            int n = 0;
            foreach (Transform child in transform)
            {
                children[n++] = child;
            }
            Array.Sort(children, new TransformNameComparer());
            waypointList.items = new Transform[children.Length];
            for (n = 0; n < children.Length; ++n)
            {
                waypointList.items[n] = children[n];
            }
        }

        void AutoRename()
        {

            int n = 0;
            foreach (Transform child in waypointList.items)
            {
                child.name = "Waypoint " + (n++).ToString("000");
            }
        }

        public RoutePoint GetRoutePoint(float dist)
        {
            // position and direction
            Vector3 p1 = GetRoutePosition(dist);
            Vector3 p2 = GetRoutePosition(dist + 0.1f);
            Vector3 delta = p2 - p1;
            return new RoutePoint(p1, delta.normalized);
        }

        public Vector3 EndPoint(Vector3 currentPosition)
        {
            if (waypointList.items.Length == 0)
                return currentPosition;
            return waypointList.items[0].position;
        }

        public bool IsFullLap(float dist)
        {
            float totalCircuitLength = 0;
            if(distances.Length != 0)
                totalCircuitLength = distances[distances.Length - 1];

            //Debug.Log("Travelled " + dist + "/" + totalCircuitLength);

            if (dist >= totalCircuitLength)
                return true;
            return false;
        }

        public int GetRoutePointIndex(float dist)
        {
            CachePositionsAndDistances();
            int p = 0;

            //The final point in distances is always zero

            //starting at the final waypoint and going backwards
            for (int i = distances.Length - 2; i > -1; i--)
            {
                //save the current point as the return
                p = i;
                //if the distance is greater than the distance[savedPoint]
                //return the saved point
                if (dist >= distances[i])
                    return p;
            }

            return p;
        }

        public void ReversePathPoints()
        {
            if (waypointList.items.Length <= 1)
                return;
            
            List<Transform> buf = new List<Transform>();
            buf.Add(waypointList.items[0]);

            for (int i = waypointList.items.Length - 1; i > 0; i--)
            {

                buf.Add(waypointList.items[i]);

            }
            waypointList.items = buf.ToArray();
        }

        public Vector3 GetRoutePosition(float dist)
        {
            int point = 0;

            if (Length == 0)
            {
                Length = distances[distances.Length - 1];
            }

            dist = Mathf.Repeat(dist, Length);

            while (distances[point] < dist)
            {
                ++point;
            }


            // get nearest two points, ensuring points wrap-around start & end of circuit
            p1n = ((point - 1) + numPoints) % numPoints;
            p2n = point;

            // found point numbers, now find interpolation value between the two middle points

            i = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);

            if (smoothRoute)
            {
                // smooth catmull-rom calculation between the two relevant points


                // get indices for the surrounding 2 points, because
                // four points are required by the catmull-rom function
                p0n = ((point - 2) + numPoints) % numPoints;
                p3n = (point + 1) % numPoints;

                // 2nd point may have been the 'last' point - a dupe of the first,
                // (to give a value of max track distance instead of zero)
                // but now it must be wrapped back to zero if that was the case.
                p2n = p2n % numPoints;

                P0 = points[p0n];
                P1 = points[p1n];
                P2 = points[p2n];
                P3 = points[p3n];

                return CatmullRom(P0, P1, P2, P3, i);
            }
            else
            {
                // simple linear lerp between the two points:

                p1n = ((point - 1) + numPoints) % numPoints;
                p2n = point;

                return Vector3.Lerp(points[p1n], points[p2n], i);
            }
        }


        private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
        {
            // comments are no use here... it's the catmull-rom equation.
            // Un-magic this, lord vector!
            return 0.5f *
                   ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                    (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
        }


        private void CachePositionsAndDistances()
        {
            // transfer the position of each point and distances between points to arrays for
            // speed of lookup at runtime
            points = new Vector3[Waypoints.Length + 1];
            distances = new float[Waypoints.Length + 1];

            float accumulateDistance = 0;
            for (int i = 0; i < points.Length; ++i)
            {
                var t1 = Waypoints[(i) % Waypoints.Length];
                var t2 = Waypoints[(i + 1) % Waypoints.Length];
                if (t1 != null && t2 != null)
                {
                    Vector3 p1 = t1.position;
                    Vector3 p2 = t2.position;
                    points[i] = Waypoints[i % Waypoints.Length].position;
                    distances[i] = accumulateDistance;
                    accumulateDistance += (p1 - p2).magnitude;
                }
            }
        }


        private void OnDrawGizmos()
        {
            DrawGizmos(false);
        }


        private void OnDrawGizmosSelected()
        {
            DrawGizmos(true);
        }


        private void DrawGizmos(bool selected)
        {
            if (Waypoints.Length > 1)
            {
                numPoints = Waypoints.Length;

                CachePositionsAndDistances();
                Length = distances[distances.Length - 1];

                Vector3 prev = Waypoints[0].position;
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(prev, nfSphereSize);
                Gizmos.color = selected ? Color.yellow : new Color(1, 1, 0, 0.5f);


                if (smoothRoute)
                {
                    for (float dist = 0; dist < Length; dist += Length / editorVisualisationSubsteps)
                    {
                        Vector3 next = GetRoutePosition(dist + 1);
                        Gizmos.DrawLine(prev, next);
                        prev = next;
                    }
                    Gizmos.DrawLine(prev, Waypoints[0].position);
                }

                for (int n = 0; n < Waypoints.Length; ++n)
                {
                    Vector3 next = Waypoints[(n + 1) % Waypoints.Length].position;
                    if (smoothRoute == false)
                    {
                        Gizmos.DrawLine(prev, next);
                    }
                    if (n == 0)
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(prev, nfSphereSize);
                    Gizmos.color = Color.yellow;
                    prev = next;
                }



            }
        }

        public class TransformNameComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return ((Transform)x).name.CompareTo(((Transform)y).name);
            }
        }

    }
}

