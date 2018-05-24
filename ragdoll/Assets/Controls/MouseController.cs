using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class MouseController : NetworkBehaviour
{
    [HideInInspector]
    public Button Lmb;

    [SerializeField]
    protected UnityEvent OnLmbDown;
    [SerializeField]
    protected UnityEvent OnLmbUp;

    private void Start()
    {
        Lmb = new Button();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Lmb.IsDown == false && Input.GetMouseButton(0) == true)
        {
            Lmb.Down();
            if (OnLmbDown != null) OnLmbDown.Invoke();
        }
        else if (Lmb.IsDown == true && Input.GetMouseButton(0) == false)
        {
            Lmb.Up();
            if (OnLmbUp != null) OnLmbUp.Invoke();
        }
    }

    public class Button
    {
        private bool isDown = false;
        private float timeDown = 0;
        private float timeUp = 0;
        private Vector2 positionDown = Vector2.zero;
        private Vector2 positionUp = Vector2.zero;

        public bool IsDown
        {
            get { return isDown; }
        }

        public float TimeDown
        {
            get { return timeDown; }
        }

        public float TimeUp
        {
            get { return timeUp; }
        }

        public float TimeHeld
        {
            get
            {
                if (IsDown == true)
                {
                    return Time.time - timeDown;
                }
                else
                {
                    return TimeUp - timeDown;
                }
            }
        }

        public Vector2 PositionDown
        {
            get { return positionDown; }
        }

        public Vector2 PositionUp
        {
            get { return positionUp; }
        }

        public Vector2 Position
        {
            get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); }
        }

        public Vector2 Path
        {
            get
            {
                if (IsDown == true)
                {
                    return (
                        new Vector2(
                            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                            Camera.main.ScreenToWorldPoint(Input.mousePosition).y)
                            - PositionDown);
                }
                else
                {
                    return (PositionUp - PositionDown);
                }
            }
        }

        public bool IsDrag()
        {
            return Path.magnitude >= 0.2;
        }

        public bool IsClick()
        {
            return Path.magnitude < 0.2;
        }

        public void Down()
        {
            isDown = true;
            timeDown = Time.time;
            positionDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void Up()
        {
            isDown = false;
            timeUp = Time.time;
            positionUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}