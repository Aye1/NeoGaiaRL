using UnityEngine;

public class Player : Character {

    public bool has10Jumps = false;

	// Use this for initialization
	void Start () {
        GameManager.instance.AddCharacter(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
