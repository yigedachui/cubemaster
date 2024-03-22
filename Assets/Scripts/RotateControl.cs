using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeanFinger
{
    public bool Up { get; internal set; }
    public bool Set { get; internal set; }
    public bool LastSet { get; internal set; }
    public bool Tap { get; internal set; }
    public bool Swipe { get; internal set; }

    public int Index;
    internal Vector2 StartScreenPosition;
    public Vector2 LastScreenPosition;
    internal Vector2 ScreenPosition;
    public float LastPressure;
    internal float Pressure;
}

public class RotateControl : MonoBehaviour
{
    public List<LeanFinger> Fingers = new List<LeanFinger>();

    public float CurrentDpi = 50;

    public Vector2 lastScreenPosition;
    public Vector2 ScreenPosition;

    public float ScalingFactor
    {
        get
        {
            var dpi = Screen.dpi;
            if (dpi <= 0)
            {
                dpi = 1.0f;
            }

            return CurrentDpi / dpi;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    Vector2 LastPos;
    Vector2 Dir;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {

            }
            if (touch.phase == TouchPhase.Stationary)
            {

            }

            if (touch.phase == TouchPhase.Moved)
            {

            }

            Vector2 delta = touch.deltaPosition;
            
            transform.Rotate(Vector3.down, delta.x * ScalingFactor, Space.World);
            transform.Rotate(Vector3.right, delta.y * ScalingFactor, Space.World);
        }


        //UpdateFingers(Time.deltaTime, true);
    }

    private void UpdateFingers(float deltaTime, bool b)
    {

        BeginFingers(deltaTime);

        PollFingers();
    }

    private void PollFingers()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);

                int id = touch.fingerId;
                Vector2 position = touch.position;
                float pressure = touch.pressure;
                bool set = touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary
                    || touch.phase == TouchPhase.Moved;

                AddFinger(id, position, pressure, set);
            }
        }
    }

    private LeanFinger AddFinger(int index, Vector2 screenPosition, float pressure, bool set)
    {
        LeanFinger finger = FindFinger(index);

        if (set == false)
        {
            return null;
        }

        if (finger == null)
        {
            finger = new LeanFinger();  
            finger.Index = index;
        }

        finger.StartScreenPosition = screenPosition;
        finger.LastScreenPosition = screenPosition;
        finger.LastPressure = pressure;
        Fingers.Add(finger);

        finger.Set = set;
        finger.ScreenPosition = screenPosition;
        finger.Pressure = pressure;

        return finger;
    }

    private LeanFinger FindFinger(int idnex)
    {
        foreach (var finger in Fingers)
        {
            if (finger.Index == idnex)
            {
                return finger;
            }
        }   
        return null;
    }

    private void BeginFingers(float deltaTime)
    {
        for (int i = Fingers.Count - 1; i >= 0; i++)
        {
            var finger = Fingers[i];
            if (finger.Up == true|| finger.Set == false)
            {
                Fingers.RemoveAt(i);
            }
            else
            {
                finger.LastSet = finger.Set;
                finger.LastPressure = finger.Pressure;
                finger.LastScreenPosition = finger.ScreenPosition;

                finger.Set = false;
                finger.Tap = false;
                finger.Swipe = false;
            }


            
        }
    }
}
