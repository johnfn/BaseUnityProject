using UnityEngine;
using System.Collections;

public enum Type
{
    Normal,
    Wall
}

public class TileModel: BaseModel
{
    public TileModel()
    {
        TileType = Type.Normal;
    }

    private Type _TileType;
    public Type TileType
    {
        get
        {
            return _TileType;
        }
        set
        {
            if (_TileType != value) Dirty = true;
            _TileType = value;
        }
    }
}

public class Tile: BaseBehavior<TileModel> {
    private BoxCollider2D collider;

    private SpriteRenderer renderer;

    public float Width { get { return collider.bounds.size.x; } }

    public float Height { get { return collider.bounds.size.y; } }

    public Vector2 Dimensions { get { return new Vector2(Width, Height); } }

	// Use this for initialization
	void Awake()
	{
	    Model = new TileModel();

        collider = GetComponent<BoxCollider2D>();
	    renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	new void Update()
	{
	    base.Update();
	}

    override protected void DirtyUpdate()
    {
        if (Model.TileType == Type.Normal)
        {
            renderer.color = Color.white;
        }

        if (Model.TileType == Type.Wall)
        {
            renderer.color = Color.red;
        }
    }

    void OnMouseOver()
    {
        Model.TileType = Type.Wall;
    }

    void OnMouseExit()
    {
        Model.TileType = Type.Normal;
    }
}
