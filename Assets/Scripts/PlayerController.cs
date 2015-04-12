using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class PlayerModel : BaseModel
{
    
}

public class PlayerController : BaseBehavior<PlayerModel>
{
    private BoxCollider2D _collider;

    private Transform _transform;

    public float Width { get { return _collider.bounds.size.x; } }

    public float Height { get { return _collider.bounds.size.y; } }

    public LayerMask WallMask;

    private bool OnGround = false;

    private Vector3 _velocity;

    private ControllableStats _stats;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _transform = GetComponent<Transform>();
        _stats = GetComponent<ControllableStats>();

        Model = new PlayerModel();
        _velocity = Vector3.zero;
    }

    float HorizontalForce()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) return -1.0f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) return 1.0f;

        return 0.0f;
    }

	void Start() 
    {
	}

    [UsedImplicitly]
	new void Update()
	{
	    base.Update();

        ApplyFriction(ref _velocity);
        UpdateVelocity(ref _velocity);
        CapVelocity(ref _velocity);
        CheckForCollisions(ref _velocity);

        _transform.Translate(_velocity);

        if (_transform.localScale != Vector3.one)
        {
            Debug.LogError("localScale needs to be 1/1/1 or else collision is sad.");
        }
	}

    void ApplyFriction(ref Vector3 velocity)
    {
        if (Math.Abs(velocity.x) < _stats.Friction)
        {
            velocity.x = 0;
        }
        else
        {
            velocity.x -= Math.Sign(velocity.x) * _stats.Friction;
        }
    }

    void CapVelocity(ref Vector3 velocity)
    {
        if (Math.Abs(velocity.x) > _stats.MaxHorizontalSpeed)
        {
            velocity.x = Math.Sign(velocity.x) * _stats.MaxHorizontalSpeed;
        }
    }

    void UpdateVelocity(ref Vector3 velocity)
    {
        velocity.x += HorizontalForce() * _stats.HorizontalSpeed * Time.deltaTime;

        velocity.y -= _stats.Gravity * Time.deltaTime;

        if (OnGround && Math.Abs(Input.GetAxis("Jump")) > .001f)
        {
            velocity.y = Input.GetAxis("Jump") * _stats.JumpHeight / 60;
        }
    }

    void CheckForCollisions(ref Vector3 velocity)
    {
        // Check for vertical collisions
        const int numRays = 8;
        const float skinWidth = .01f;

        if (Math.Abs(velocity.y) > .0001f)
        {
            OnGround = false;

            var firstRayOrigin = transform.position + new Vector3(-Width / 2, Math.Sign(velocity.y) * Height / 2, 0.0f);
            var rayDirection = Vector2.up * -Math.Sign(velocity.y);

            for (var i = 0; i < numRays; i++)
            {
                var xOffset = i * (Width - skinWidth * 2) / (numRays - 1) + skinWidth;
                var rayOrigin = firstRayOrigin + new Vector3(xOffset, 0.0f, 0.0f);
                var raycastHit = Physics2D.Raycast(rayOrigin, rayDirection, velocity.y, WallMask);

                Debug.DrawRay(rayOrigin, rayDirection * velocity.y, Color.red);

                if (!raycastHit) continue;

                // To get the thing you're touching: raycastHit.collider.gameObject

                OnGround = OnGround || (velocity.y < 0);

                var newVelocity = raycastHit.point.y - rayOrigin.y;

                if (Math.Abs(newVelocity) < Math.Abs(velocity.y))
                {
                    velocity.y = newVelocity;
                }
            }
        }

        // Check for horizontal collisions

        if (Math.Abs(velocity.x) > .0001f)
        {
            var firstRayOrigin = transform.position + new Vector3(Math.Sign(velocity.x) * Width / 2, -Height / 2, 0.0f);
            var rayDirection = Vector2.right * Math.Sign(velocity.x);

            for (var i = 0; i < numRays; i++)
            {
                var yOffset = i * (Height - skinWidth * 2) / (numRays - 1) + skinWidth;
                var rayOrigin = firstRayOrigin + new Vector3(0.0f, yOffset, 0.0f);
                var raycastHit = Physics2D.Raycast(rayOrigin, rayDirection, Math.Abs(velocity.x), WallMask);

                Debug.DrawRay(rayOrigin, rayDirection * Math.Abs(velocity.x) * 20, Color.red);

                if (!raycastHit) continue;

                var newVelocity = raycastHit.point.x - rayOrigin.x;

                if (Math.Abs(newVelocity) < Math.Abs(velocity.x))
                {
                    velocity.x = newVelocity;
                }
            }
        }
    }

    protected override void DirtyUpdate()
    {

    }
}
