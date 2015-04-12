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

    private Vector3 BottomRayOrigin, LeftRayOrigin, RightRayOrigin;

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

    void CalculateRayOrigins()
    {
        BottomRayOrigin = transform.position + new Vector3(0.0f, -Height / 2, 0.0f);
        LeftRayOrigin = transform.position + new Vector3(-Width / 2, 0.0f, 0.0f);
        RightRayOrigin = transform.position + new Vector3(Width / 2, 0.0f, 0.0f);
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

        CalculateRayOrigins();

        UpdateVelocity(ref _velocity);
        ApplyFriction(ref _velocity);
        CheckForCollisions(ref _velocity);

        _transform.Translate(_velocity);
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

        if (Math.Abs(velocity.y) > .0001f)
        {
            OnGround = false;

            var firstRayOrigin = transform.position + new Vector3(-Width / 2, -Height / 2, 0.0f) * -Math.Sign(velocity.y);

            for (var i = 0; i < numRays; i++)
            {
                var rayOrigin = firstRayOrigin + new Vector3(i * Width / (numRays - 1), 0.0f, 0.0f);
                // TODO: Figure out actual direction, not just always down.
                var raycastHit = Physics2D.Raycast(rayOrigin, -Vector2.up, velocity.y, WallMask);

                Debug.DrawRay(rayOrigin, -Vector2.up, Color.red);

                if (!raycastHit) continue;

                OnGround = OnGround || (velocity.y < 0);

                var newPosition = raycastHit.point.y - rayOrigin.y;

                if (newPosition > velocity.y)
                {
                    velocity.y = newPosition;
                }
            }
        }

        // Check for horizontal collisions

        if (Math.Abs(velocity.x) > .0001f)
        {
            var dx = Math.Abs(velocity.x);
            var rayOrigin = velocity.x > 0 ? RightRayOrigin : LeftRayOrigin;
            var rayDirection = Vector2.right * Math.Sign(velocity.x);

            Debug.DrawRay(rayOrigin, rayDirection * dx * 10, Color.red);

            var raycastHit = Physics2D.Raycast(rayOrigin, rayDirection, dx, WallMask);
            if (raycastHit)
            {
                velocity.x = raycastHit.point.x - rayOrigin.x;
            }
        }
    }

    protected override void DirtyUpdate()
    {

    }
}
