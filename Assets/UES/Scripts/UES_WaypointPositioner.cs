using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

[ExecuteInEditMode]
public class UES_WaypointPositioner : MonoBehaviour
{
    public float riseSpeed = 1, fallSpeed = 0.35F;
    public GameObject positionedObject;
    public WaypointCircuit backBar;

    float dist = 0, distTravelled = 0;
    enum Movement { Still, Rising, Falling }
    Movement currentMovement = Movement.Still;

    public float percent = 0;

    private void Update()
    {
        if (Application.isEditor)
            FixedUpdate();
    }

    public void PlaceDisc(float impactFactor)
    {
        percent = impactFactor;

        Debug.Log(percent);

        distTravelled = 0;

        currentMovement = Movement.Rising;
        Debug.Log("Going to Rising.");

        //The Waypoint circuit's Length function returns a completed loop, making it twice as long
        //for two point paths...for some reason.
        float magicDivider = 2;
        dist = backBar.Length * percent / magicDivider;

    }

    public void FixedUpdate()
    {
        if (percent > 1)
            percent = 1;
        if (percent < 0)
            percent = 0;

        if (positionedObject == null || backBar == null)
            return;

        //if (Application.isEditor)
        //{
        //    positionedObject.transform.position = backBar.GetRoutePoint(percent).position;
        //    return;
        //}

        switch (currentMovement)
        {
            case Movement.Still:
                positionedObject.transform.position = backBar.GetRoutePoint(0).position;
                break;
            case Movement.Rising:
                distTravelled += Time.deltaTime * riseSpeed;
                if (distTravelled >= dist)
                {
                    distTravelled = dist;
                    currentMovement = Movement.Falling;

                    Debug.Log("Going to Falling.");
                }

                positionedObject.transform.position = backBar.GetRoutePoint(distTravelled).position;
                break;

            case Movement.Falling:
                distTravelled -= Time.deltaTime * fallSpeed;

                if (distTravelled <= 0)
                {
                    distTravelled = 0;
                    dist = 0;
                    currentMovement = Movement.Still;
                    positionedObject.transform.position = backBar.GetRoutePoint(0).position;
                    Debug.Log("Going to Still.");
                }
                positionedObject.transform.position = backBar.GetRoutePoint(distTravelled).position;

                break;
        }
    }
}
