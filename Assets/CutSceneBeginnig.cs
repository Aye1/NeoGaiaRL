using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneBeginnig : MonoBehaviour {

    private bool _isCutsceneStarted = false;
    private bool _isCutsceneFinished = false;

    private Player _player;
    private CameraBehaviour _camera;
    public List<Vector3> camPositions;
    public List<float> times;

	// Use this for initialization
	void Start () {
        _player = FindObjectOfType<Player>();
        _camera = FindObjectOfType<CameraBehaviour>();
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCutscene();
        }
    }

    private void StartCutscene()
    {
        Debug.Log("Start cutscene");
        _isCutsceneStarted = true;
        _player.canMove = false;
        _camera.isCinematicMode = true;
        StartCoroutine(MoveCamera());
    }

    private void EndCutScene()
    {
        _isCutsceneFinished = true;
        _player.canMove = true;
        _camera.isCinematicMode = false;
    }

    private IEnumerator MoveCamera()
    {
        int i = 0;
        foreach(Vector3 pos in camPositions)
        {
            Vector3 newPos = _camera.transform.position + pos;
            float time = times[i];
            i++;
            _camera.GoToPosition(newPos, time);
            yield return new WaitForSeconds(time);
        }
    }
}
