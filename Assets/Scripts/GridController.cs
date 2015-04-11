using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {

	void Start() {
        Vector2 startPosition = this.transform.position;

        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                var tile = Manager.CreateTile();

                tile.transform.position = startPosition + tile.Dimensions.Multiply(new Vector2(i, -j));
            }
        }
	}
	
	// Update is called once per frame
	void Update() {
	
	}
}
