using System.Collections.Generic;
using Extensions;
using Game_Objects;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SavedEntry body;
    public SavedEntry head;
    public SavedEntry rightArm;
    public SavedEntry leftArm;
    public SavedEntry rightLeg;
    public SavedEntry leftLeg;
    public Camera cam;
    private float _legTime;
    public float speed;
    public float boatSpeed;
    
    public Transform held;

    private bool _legsMoving;
    public Rigidbody2D myRigidbody;

    public Game game;
    public bool saddled;
    public bool onBoat;
    public bool onOx;

    public float angle;
    private float _desiredRightAngle;
    private float _desiredLeftAngle;
    private float _blinkRate;
    private bool _blinking;

    private const float MAXLegTime = 0.5f;
    private const float MAXBlink = 1f;
    private Water _water;

    public bool armed;

    private void Start()
    {
        body.Animate(PlayerInfo.Pass);
        head.Animate(PlayerInfo.Pass);
        rightArm.Animate(PlayerInfo.Pass);
        leftArm.Animate(PlayerInfo.Pass);
        rightLeg.Animate(PlayerInfo.Pass);
        leftLeg.Animate(PlayerInfo.Pass);
        //_direction = 1;
        _blinkRate = Random.Range(3f, 4f);
        myRigidbody.gravityScale = 20;
    }

    private void Update()
    {
        Move();
        Breathe();
        Blink();
        GetOnBoat();
        RotateSpear();
        Step();
        Spawn();
        GetFromBoat();
        TryDie();

    }

    private void TryDie()
    {
        if(transform.position.y<-30)
            Die();
    }

    private void RotateSpear()
    {
        if (!armed)
            return;
        var mpos = cam.ScreenToWorldPoint(Input.mousePosition);
        held.localEulerAngles = held.localEulerAngles.WithZ(Mathf.Atan2(mpos.y, mpos.x));
    }

    private void GetFromBoat()
    {
        if (!Input.GetKeyDown(KeyCode.Tab) || !onBoat)
            return;
        _water.withPlayer = false;
        transform.SetParent(_water.transform.parent);
        _water = null;
        onBoat = false;
        myRigidbody.simulated = true;
        myRigidbody.AddForce(new Vector2(Mathf.Abs(transform.localEulerAngles.y) < 1 ? 3000 : -3000, 3000));
    }

    private void GetOnBoat()
    {
        if (!(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) || onBoat)
            return;
        var water = FindObjectOfType<Water>();
        if (water == null || water.boat == null)
            return;
        _water = water;
        _water.withPlayer = true;
        transform.SetParent(water.boat);
        transform.localPosition = new Vector3(0, 8, 0);
        myRigidbody.simulated = false;
        onBoat = true;
    }

    private void TurnBack(Component turning)
    {
        turning.transform.localEulerAngles = new Vector3();
    }

    private void Step()
    {
        if (!_legsMoving)
        {
            TurnBack(leftLeg);
            TurnBack(rightArm);
            TurnBack(rightLeg);
            TurnBack(leftArm);
            _legTime = 0;
            return;
        }
        if (onBoat)
            return;
        var dt = Time.deltaTime;
        _desiredLeftAngle = -angle * LinSin(_legTime / MAXLegTime);
        _desiredRightAngle = angle * LinSin(_legTime / MAXLegTime);

        leftLeg.transform.localEulerAngles += _desiredLeftAngle / MAXLegTime * dt * Vector3.forward;
        rightArm.transform.localEulerAngles +=
            _desiredLeftAngle / MAXLegTime * dt * Vector3.forward; // don't ask just believe
        rightLeg.transform.localEulerAngles += _desiredRightAngle / MAXLegTime * dt * Vector3.forward;
        leftArm.transform.localEulerAngles += _desiredRightAngle / MAXLegTime * dt * Vector3.forward; // same

        _legTime += dt;
    }

    private void Blink()
    {
        var dt = Time.deltaTime;
        _blinkRate -= dt;
        if (_blinking)
        {
            // le close|le opens|le stays
            // --------|--------|--------
            // re stays|re close|re opens
            var leftEye = head.transform.GetChild(1);
            leftEye.localScale = new Vector3(1,
                _blinkRate > 2 * MAXBlink / 3
                    ? 1
                    : 0.5f + Mathf.Abs(_blinkRate - MAXBlink / 3) / MAXBlink * 3 / 2,
                1);
            var rightEye = head.transform.GetChild(2);
            rightEye.localScale = new Vector3(
                1,
                _blinkRate < MAXBlink / 3
                    ? 1
                    : 0.5f + Mathf.Abs(_blinkRate - MAXBlink * 2 / 3) / MAXBlink * 3 / 2,
                1);
        }

        if (_blinkRate >= 0)
            return;
        if (!_blinking)
        {
            _blinking = true;
            _blinkRate = MAXBlink; //Random.Range(1f, 2f);
        }
        else
        {
            _blinking = false;
            _blinkRate = Random.Range(2f, 4f);
        }
    }

    private void Breathe()
    {
        transform.localScale = new Vector3(transform.localScale.x, 1 + Mathf.Sin(Time.time) / 120f, 1);
    }

    public void Die()
    {
        PlayerInfo.Skulls.Add(transform.position);
        game.SpawnSkull(transform.position);
        PlayerInfo.Save();
        Destroy(gameObject);
        FindObjectOfType<LevelLoader>().SelectScene("Dead Scene");
    }

    private void Spawn()
    {
        if (onBoat || onOx)
            return;
        if (!Input.GetMouseButtonDown(1) || !PlayerInfo.CanSpawnCurrentType)
            return;
        var p = cam.ScreenToWorldPoint(Input.mousePosition);
        p.z = -15;
        var type = PlayerInfo.CurrentType;
        game.Spawn(type, p);
    }

    private void Move()
    {
        var pressDir = Input.GetAxis("Horizontal");
        if (onOx)
            return;
        transform.localEulerAngles = pressDir > 0
            ? new Vector3(0, 0, 0)
            : pressDir < 0
                ? new Vector3(0, 180, 0)
                : transform.localEulerAngles;
        if (onBoat && !_water.strongStream)
        {
            _water.boat.position += pressDir * Time.fixedDeltaTime * boatSpeed * Vector3.right;
            return;
        }

        myRigidbody.MovePosition(myRigidbody.position +
                                 new Vector2(pressDir * Time.fixedDeltaTime * speed, 0));

        _legsMoving = Mathf.Abs(pressDir) >= 0.1f;
    }

    private float LinSin(float x)
    {
        var res = x % 4;
        if (res < 1)
            return res;
        if (res > 3)
            return res - 4;
        return 2 - res;
    }
}