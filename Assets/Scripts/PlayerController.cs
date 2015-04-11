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

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _transform = GetComponent<Transform>();

        Model = new PlayerModel();

        CalculateRayOrigins();
    }

    void CalculateRayOrigins()
    {
        BottomRayOrigin = transform.position + new Vector3(0.0f, -Height / 2, 0.0f);
        LeftRayOrigin = transform.position + new Vector3(-Width / 2, 0.0f, 0.0f);
        RightRayOrigin = transform.position + new Vector3(Width / 2, 0.0f, 0.0f);
    }

	void Start() 
    {
	}

    [UsedImplicitly]
	new void Update()
	{
	    base.Update();

        var movement = new Vector3(Input.GetAxis("Horizontal") * 10, -1, 0) * Time.deltaTime;

        CalculateRayOrigins();
        CheckForCollisions(movement);
	}

    void CheckForCollisions(Vector3 movement)
    {
        // Check for vertical collisions

        if (Math.Abs(movement.y) > .0001f)
        {
            var rayOrigin = transform.position + new Vector3(0.0f, -Height/2, 0.0f);
            // Debug.DrawRay(rayOrigin, (Vector2.up * -1), Color.red);

            var raycastHit = Physics2D.Raycast(rayOrigin, Vector2.up, movement.y, WallMask);
            if (raycastHit)
            {
                movement.y = raycastHit.point.y - rayOrigin.y;
            }
        }

        // Check for horizontal collisions

        if (Math.Abs(movement.x) > .0001f)
        {
            var rayOrigin = movement.x > 0 ? RightRayOrigin : LeftRayOrigin;
            var rayDirection = Vector2.right;
            Debug.DrawRay(rayOrigin, rayDirection * 1000, Color.red);

            var raycastHit = Physics2D.Raycast(rayOrigin, rayDirection, movement.x, WallMask);
            if (raycastHit)
            {
                movement.x = raycastHit.point.x - rayOrigin.x;
            }
        }

        _transform.Translate(movement);
    }

    protected override void DirtyUpdate()
    {

    }
}
