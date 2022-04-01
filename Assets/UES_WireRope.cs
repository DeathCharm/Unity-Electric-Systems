using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UES_WireRope : MonoBehaviour
{
    public float hang = 0.5F, vertexCount = 3, width = 0.25F;
    public LineRenderer rope;
    public List<Vector3> ropeLocalPositions = new List<Vector3>();

    public Color emissionColor = Color.blue;
    public ARX_Script_IndividualColor mo_colorer;
    public GameObject mo_endObject;
    ARX_Script_IndividualColor GetColorer
    {
        get
        {
            if (mo_colorer == null)
                mo_colorer = GetComponent<ARX_Script_IndividualColor>();
            return mo_colorer;
        }
    }

    ARX.UnityTimer timer = new ARX.UnityTimer();

    private void FixedUpdate()
    {
        if (timer.mb_active == false)
            return;

        timer.Tick();
        if (timer.IsFinished)
        {
            OnPowerDown();
            timer.mb_active = false;
        }
    }

    private void Start()
    {
        OnPowerDown();
    }

    private void Update()
    {
        rope.widthCurve = new AnimationCurve(new Keyframe(1, width));
        UpdateRopePositions();
        GetColorer.mo_color = emissionColor;

        if (ropeLocalPositions.Count < 1)
        {
            ropeLocalPositions.Add(Vector3.zero);
        }
        if (mo_endObject != null && ropeLocalPositions.Count < 2)
        {
            ropeLocalPositions.Add(Vector3.zero);
        }
        if (mo_endObject != null && ropeLocalPositions.Count > 1)
        {
            int nLastPosition = ropeLocalPositions.Count - 1;
            ropeLocalPositions.Remove(ropeLocalPositions[nLastPosition]);
            Vector3 dif = mo_endObject.transform.position - transform.position;
            ropeLocalPositions.Add(dif);
        }
    }

    public List<Vector3> GetTranslatedRopePositions()
    {
        List<Vector3> bufRopePivotPoints = new List<Vector3>(ropeLocalPositions);
        if (bufRopePivotPoints.Count <= 1)
            return bufRopePivotPoints;

        var pointList = new List<Vector3>();

        //Add this object's current position to the points
        //since they are local positions
        for (int i = 0; i < bufRopePivotPoints.Count; i++)
        {
            bufRopePivotPoints[i] += transform.position;
        }

        //Create curved sub points
        for (int i = 0; i < bufRopePivotPoints.Count -1; i++)
        {
            Vector3 Point1, Point2, Point3;

            Point1 = bufRopePivotPoints[i];
            Point3 = bufRopePivotPoints[i + 1];
            
            float Point2Ypositio = Point3.y - hang;

            Point2 = new Vector3((Point1.x + Point3.x)/2, Point2Ypositio, (Point1.z + Point3.z) / 2);

            for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
            {
                var tangent1 = Vector3.Lerp(Point1, Point2, ratio);
                var tangent2 = Vector3.Lerp(Point2, Point3, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);

                pointList.Add(curve);
            }
        }


        ////Create the curve of the last point
        //{
        //    Vector3 Point1, Point2, Point3;
        //    Point1 = pointList[pointList.Count - 1];

        //    Point3 = bufRopePivotPoints[bufRopePivotPoints.Count - 1];

        //    float Point2Ypositio = Point3.y - hang;

        //    Point2 = new Vector3((Point1.x + Point3.x), Point2Ypositio, (Point1.z + Point3.z) / 2);

        //    for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
        //    {
        //        var tangent1 = Vector3.Lerp(Point1, Point2, ratio);
        //        var tangent2 = Vector3.Lerp(Point2, Point3, ratio);
        //        var curve = Vector3.Lerp(tangent1, tangent2, ratio);

        //        pointList.Add(curve);
        //    }
        //}

        pointList.Add(bufRopePivotPoints[bufRopePivotPoints.Count - 1]);



        return pointList;
    }

    private void UpdateRopePositions()
    {
        List<Vector3> bufRope = GetTranslatedRopePositions();

        rope.positionCount = bufRope.Count;
        rope.SetPositions(bufRope.ToArray());
    }

    private void OnDrawGizmos()
    {
        float gizmoWidth = 0.05F;

        Gizmos.DrawSphere(transform.position, gizmoWidth);
    }

    public void OnPowerUp() {
        emissionColor = Color.blue;
    }
    public void OnPowerDown() {
        emissionColor = Color.black;
    }

    public void OnTriggered()
    {
        emissionColor = Color.green;
        timer.Start(0.1F);
    }

}