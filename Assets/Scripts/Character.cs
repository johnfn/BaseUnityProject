using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
    private ControllableStats _stats;
    private PhysicsController2D _physics;

	void Start() {
        // TODO move friction and vel cap in here, too.

	    _physics = GetComponent<PhysicsController2D>();
	    _stats = GetComponent<ControllableStats>();
	}
	
    void UpdateVelocity()
    {
	    var collision = GetComponent<PhysicsController2D>().Collisions;
        var horizontalForce = 0.0f;
        var velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) horizontalForce = -1.0f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) horizontalForce = 1.0f;

        velocity.x += horizontalForce * _stats.HorizontalSpeed * Time.deltaTime;

        if (collision.TouchingBottom && Input.GetKey(KeyCode.Space))
        {
            _physics.SetVerticalForce(_stats.JumpHeight / 60);
        }

        if (!collision.TouchingBottom && !Input.GetKey(KeyCode.Space) && velocity.y > 0)
        {
            _physics.SetVerticalForce(0);
        }

        _physics.AddHorizontalForce(velocity.x);
        _physics.AddVerticalForce(velocity.y);
    }


	void Update()
	{
	    var collision = GetComponent<PhysicsController2D>().Collisions;

	    UpdateVelocity();

	    foreach (Collision t in collision.PreviouslyTouchedObjects)
	    {
	        if (!t.Object.GetComponent<SpriteRenderer>()) continue; 

	        t.Object.GetComponent<SpriteRenderer>().color = Color.white;
	    }

	    foreach (Collision t in collision.TouchedObjects)
	    {
	        if (!t.Object.GetComponent<SpriteRenderer>()) continue;


	        t.Object.GetComponent<SpriteRenderer>().color = Color.red;

	        if (t.Side == CollisionSide.Left || t.Side == CollisionSide.Right)
	        {
	            t.Object.GetComponent<PhysicsController2D>().AddHorizontalForce(_physics.Velocity.x * 0.95f);
	        }
	    }

	    if (collision.JustTouchingBottom)
	    {
	        print("Ding");
	    }
	}
}
