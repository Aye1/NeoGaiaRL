using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneBeginning : MonoBehaviour {

    private bool _isCutsceneStarted = false;
    private bool _isCutsceneFinished = false;

    private Player _player;
    private CameraBehaviour _camera;
    public List<Movement> _cameraMove;


    public List<MovableCharacter> actors;

        [System.Serializable]
    public class MovableCharacter : System.Object
    {
        public Character character;
        public List<Movement> moves;
        
    }

    [System.Serializable]
    public class Movement : System.Object
    {
        public Vector3 position;
        public float time;
    }

    // Use this for initialization
    void Start () {
        _player = FindObjectOfType<Player>();
        _camera = FindObjectOfType<CameraBehaviour>();
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
        StartCoroutine(MoveEntity(_camera,_cameraMove));
        foreach (MovableCharacter pnj in actors)
        {
            StartCoroutine(MoveEntity(pnj.character, pnj.moves));
        }
        
    }

    private void EndCutScene()
    {
        _isCutsceneFinished = true;
        _player.canMove = true;
        _camera.isCinematicMode = false;
    }

    private IEnumerator MoveEntity(IMovable entity, List<Movement> moves)
    {
        foreach(Movement move in moves)
        {
            Vector3 newPos = entity.GetPosition() + move.position;
            entity.GoToPosition(newPos, move.time);
            yield return new WaitForSeconds(move.time);
        }
        EndCutScene();
    }
}
