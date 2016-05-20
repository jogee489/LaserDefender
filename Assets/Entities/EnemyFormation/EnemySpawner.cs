using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = 5f;
	public float spawnDelay = .5f;
	
	private bool movingRight = false;
	private float xmax, xmin;
	
	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		xmax = rightEdge.x;
		xmin = leftEdge.x;
	
		SpawnUntilFull();
	}
	
	public void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
	}
	
	// Update is called once per frame
	void Update () {
		if (movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		float rightEdgeofFormation = transform.position.x + (0.5f*width);
		float leftEdgeofFormation = transform.position.x - (0.5f*width);
		if (leftEdgeofFormation < xmin) {
			movingRight = true;
		} else if (rightEdgeofFormation > xmax) {
			movingRight = false;
		}
		if(AllMembersDead()) {
			SpawnUntilFull();
		}
	}
	
	bool AllMembersDead() {
		foreach (Transform childPositionObject in transform) {
			if (childPositionObject.childCount > 0) {
				return false;
			}
		}
		return true;
	}
	
	void SpawnEnemies() {
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}
	
	void SpawnUntilFull() {
		Transform nextPosition = NextFreePosition();
		if (nextPosition){
			GameObject enemy = Instantiate(enemyPrefab, nextPosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = nextPosition;
		}
		if (NextFreePosition()) {
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}
	
	Transform NextFreePosition() {
		foreach (Transform childPositionObject in transform) {
			if (childPositionObject.childCount == 0) {
				return childPositionObject;
			}
		}
		return null;
	}
}
