using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using ARX;

public class UES_Motor : UES_BaseModule
{
    public float cogSpeed = 2, moveSpeed = 1;

    /// <summary>
    /// The amount of time that will be elapsed pausing on a waypoint.
    /// </summary>
    public float pointDelay = 1;

    /// <summary>
    /// Loops infinitely.
    /// </summary>
    public bool loop = false;

    /// <summary>
    /// Reverses the order of its waypoints when reaching the final waypoint
    /// </summary>
    public bool reverseOnEnd = false;

    /// <summary>
    /// Pauses when first reaching a new point.
    /// </summary>
    public bool delayOnPoint = false;

    /// <summary>
    /// Sends a trigger when reaching a waypoint
    /// </summary>
    public bool triggerOnDelayStart = false;

    /// <summary>
    /// Sends a trigger when the delay payse after reaching a new
    /// waypoint ends.
    /// </summary>
    public bool triggerOnDelayEnd = false;

    /// <summary>
    /// A spining gameobject used to visually indicate when the motor is running.
    /// Purely cosmetic.
    /// </summary>
    public GameObject cog;

    /// <summary>
    /// The gameobject to be moved by the motor. If null, the motor will move its own gameobject.
    /// </summary>
    public GameObject moveTarget;

    /// <summary>
    /// 
    /// </summary>
    public GameObject pointOne, pointTwo;
    public WaypointCircuit waypointCircuit;

    public float moveElapsed = 0;
    private bool isInDelayMode = false;
    private int lastPoint = 0;
    private UnityTimer timer = new UnityTimer();

    int nLap = 0;
    public override void OnPowered()
    {
        if (cog != null)
        {
            cog.transform.Rotate(new Vector3(0, cogSpeed, 0));
        }

        if (Application.isPlaying == false)
            return;

        if (isInDelayMode)
        {
            timer.Tick();
            if (timer.IsFinished)
            {
                OnPointDelayEnd();
                isInDelayMode = false;
            }
            else
                return;
        }

        if (waypointCircuit != null)
            Move(waypointCircuit);
        else if (pointOne != null && pointTwo != null)
        {
            Move(pointOne, pointTwo);
        }

        if (waypointCircuit != null)
        {
            int nCurrentPoint = waypointCircuit.GetRoutePointIndex(moveElapsed);
            if (nCurrentPoint != lastPoint)
            {
                ReachedNewPoint();
            }

            lastPoint = nCurrentPoint;
        }
    }

    void ReachedNewPoint()
    {
        if (delayOnPoint)
        {
            OnPointDelayStart();
            timer.Start(pointDelay);
            isInDelayMode = true;
        }
        Debug.Log("At point " + lastPoint);
    }

    void OnPointDelayEnd()
    {
        if (triggerOnDelayStart)
            SendTrigger(signal);
    }
    void OnPointDelayStart()
    {
        if (triggerOnDelayStart)
            SendTrigger(signal);
    }

    private void Move(GameObject one, GameObject two)
    {
        moveElapsed += Time.deltaTime;

        Vector3 vecPos = Vector3.Lerp(one.transform.position, two.transform.position, moveElapsed);


        if (moveTarget != null)
        {
            moveTarget.transform.position = vecPos;
            if (moveTarget.transform.position == vecPos)
                EndMovement();
        }
        else
        {
            transform.position = vecPos; 
            if (transform.position == vecPos)
                EndMovement();
        }


    }
    private void Move(WaypointCircuit circuit)
    {
        moveElapsed += Time.deltaTime * moveSpeed;

        RoutePoint point = circuit.GetRoutePoint(moveElapsed);


        if(moveTarget != null)
            moveTarget.transform.position = point.position;
        else
            transform.position = point.position;

        if (circuit.IsFullLap(moveElapsed))
        {
            if (moveTarget != null)
                moveTarget.transform.position = circuit.EndPoint(transform.position);
            else
                transform.position = circuit.EndPoint(transform.position);
            EndMovement();
        }
    }

    void EndMovement()
    {
        nLap++;
        lastPoint = 0;
        moveElapsed = 0;

        if (loop == false)
            mb_isUESModuleActive = false;
        else
        {
            ReachedNewPoint();
        }
        if (reverseOnEnd && waypointCircuit != null)
            waypointCircuit.ReversePathPoints();

        SendTrigger(signal);
    }

}
