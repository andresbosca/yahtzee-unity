using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DiceManager : MonoBehaviour
{
    public struct DieData
    {
        public readonly GameObject Die;
        public Rigidbody Rigidbody;

        public DieData(GameObject die)
        {
            Die = die;
            Rigidbody = die.GetComponent<Rigidbody>();
        }
    }

    [SerializeField] private float dieDragForce;
    [SerializeField, Range(0f, 100f)] private float dieDragSmoothSpeed;
    [SerializeField, Range(0f, 1f)] private float dieMoveRandomness;
    [SerializeField] private float dieRandomForce;
    [SerializeField] private float dieRandomSmoothSpeed;
    [SerializeField] private float maxDieReleaseVelocity;

    private Camera _mainCamera;
    private int _childCount;
    private readonly List<DieData> _diceList = new();
    private readonly List<DieData> _selectedDiceList = new();
    private bool _dieAddedToSelected;
    private bool _touchingDie;
    private bool _draggingDie;
    private DieData _lastTouchedDieData;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_childCount != transform.childCount)
        {
            _childCount = transform.childCount;
            _diceList.Clear();
            
            foreach (Transform child in transform)
            {
                if (!child.name.Contains("Dice_d6"))
                    continue;
                _diceList.Add(new DieData(child.gameObject));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleDieTouch();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleDieRelease();
        }
    }

    private void FixedUpdate()
    {
        HandleDieDrag();
    }

    private void HandleDieTouch()
    {
        _dieAddedToSelected = false;
        var mousePointRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(mousePointRay, out var hit)) return;
        
        var hitObject = hit.collider.gameObject;

        var hitDieData = _diceList.FirstOrDefault(dieData => dieData.Die == hitObject);
        
        if (!hitDieData.Die) return;
        
        _touchingDie = true;
        _lastTouchedDieData = hitDieData;

        if (_selectedDiceList.Contains(hitDieData)) return;
        
        _selectedDiceList.Add(hitDieData);
        _dieAddedToSelected = true;
    }

    private void HandleDieRelease()
    {
        if (_draggingDie)
        {
            foreach (var dieData in _selectedDiceList)
            {
                dieData.Rigidbody.velocity = Vector3.ClampMagnitude(dieData.Rigidbody.velocity, maxDieReleaseVelocity);
            }
            
            _selectedDiceList.Clear();
        }
        
        _touchingDie = false;
        _draggingDie = false;
        
        if (_dieAddedToSelected) return;
        var mousePointRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(mousePointRay, out var hit)) return;
        
        var hitObject = hit.collider.gameObject;

        var hitDieData = _diceList.FirstOrDefault(dieData => dieData.Die == hitObject);
        
        if (!hitDieData.Die) return;
        
        _lastTouchedDieData = hitDieData;
        
        if (_selectedDiceList.Contains(hitDieData))
        {
            _selectedDiceList.Remove(hitDieData);
        }
    }

    private void HandleDieDrag()
    {
        if (!_touchingDie) return;
        if (!_lastTouchedDieData.Die) return;
        
        var mousePointRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(mousePointRay, out var hit)) return;

        if (!_draggingDie)
        {
            var positionDifference = hit.point - _lastTouchedDieData.Die.transform.position;

            if (positionDifference.magnitude > 0.7f)
            {
                _draggingDie = true;
            }
            
            return;
        }
        
        var hitPointOnTable = hit.point;
        hitPointOnTable.y = 1f;

        foreach (var dieData in _selectedDiceList)
        {
            var dieRb = dieData.Rigidbody;
            var dieTransform = dieData.Die.transform;

            var diePosition = dieTransform.position;
            diePosition.y = Mathf.Lerp(diePosition.y, 1f, Time.deltaTime * 5f);
            dieTransform.position = diePosition;
            
            // ReSharper disable once Unity.InefficientPropertyAccess
            var differenceToMouse = hitPointOnTable - dieTransform.position;
            var forceDirection = differenceToMouse.normalized;
            var forceLimiter = Mathf.Clamp01(differenceToMouse.magnitude);
            var randomness = Random.Range(0f, dieMoveRandomness);
            var limiter = Mathf.Clamp01(differenceToMouse.magnitude - 1f);
            var force = forceDirection * (forceLimiter * dieDragForce) + forceDirection * (dieRandomForce * randomness);
            var v = Time.deltaTime * (dieDragSmoothSpeed + dieRandomSmoothSpeed * randomness);
            dieRb.velocity = Vector3.Lerp(dieRb.velocity, force * limiter, v);
            dieRb.AddTorque(Random.insideUnitSphere * (10f * limiter));
        }
    }
}
