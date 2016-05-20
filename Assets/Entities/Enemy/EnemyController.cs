using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public GameObject laserPrefab;
	public float health = 120f;
	public float laserSpeed = 3f;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 124;
	public AudioClip fireSound;
	public AudioClip deathSound;
	
	private ScoreKeeper scoreKeeper;
	
	
	void Start () {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}
	// Update is called once per frame
	void Update () {
		float probability = shotsPerSecond * Time.deltaTime;
		if (Random.value < probability) {
			Fire ();
		}
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile) {
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0) {
				Destroy(gameObject);
				scoreKeeper.Score(scoreValue);
				AudioSource.PlayClipAtPoint(deathSound, transform.position);
			}
		}
	}
	
	void Fire() {
		Vector3 startPostition = transform.position + new Vector3(0f, -.9f, 0f);
		GameObject laser = Instantiate(laserPrefab, startPostition, Quaternion.identity) as GameObject;
		laser.rigidbody2D.velocity = new Vector3(0, -laserSpeed);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
}
