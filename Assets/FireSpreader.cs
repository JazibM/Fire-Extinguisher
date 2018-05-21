using UnityEngine;
using System.Collections;

public class FireSpreader : MonoBehaviour {

	private ParticleSystem fire;
	private ParticleSystem.EmissionModule em;
	private ParticleSystem.MinMaxCurve rate;
	private ParticleSystem.ShapeModule rad;
    private bool fireIsOut = false;
    public GameObject soldierFree = null;
     
    void OnParticleCollision(GameObject other) {
        if (other.name == "water stream") {
            // If the chosen extinguisher is bad for the fire type, then it 
            // is encouraging the fire so the fire should increase its size
            if ((tag == "Waste" && other.tag == "CO2") ||
                (tag == "Electronic" && (other.tag != "Powder" && other.tag != "CO2")) ||
                (tag == "Cooking" && other.tag != "WetChemical")) {
                animations anim = soldierFree.GetComponent<animations>();
                anim.playSound("7_what_u_doing_man_failure", false);
                Update();
                return;
            }

            float con = rate.constant;
            if (con > 0f && rad.radius > 0f) {
			    con -= 100.0f;
			    rate.constant = con;
			    em.rate = rate;
			    rad.radius -= 0.05f;

                // decrease the range of the where collision can be detected
                BoxCollider boxCollider = GetComponent<BoxCollider>();
                Vector3 colliderSize = boxCollider.size;
                colliderSize.x = rad.radius * 5.0f;
                colliderSize.y = rad.radius * 5.0f;
                boxCollider.size = colliderSize;
            }
            else if (!fireIsOut && soldierFree != null) {
                fireIsOut = true; // stop all further particle collisions from running StartGame()
                animations anim = soldierFree.GetComponent<animations>();
                anim.playSound("6_good_work_success", false);
                anim.startGame();
                soldierFree = null;
                gameObject.GetComponent<AudioSource>().Stop();
            }
        }
    }

    void Start() {
		fire = gameObject.GetComponent<ParticleSystem> ();
		em = fire.emission;
		rate = em.rate;
		rate.constantMax = 100.0f;
		em.rate = rate;
		rad = fire.shape;
		rad.enabled = true;
	}
	void Update() {
		if (soldierFree != null && !fireIsOut) {
            AudioSource sfxFire = gameObject.GetComponent<AudioSource>();
            if (!sfxFire.isPlaying) {
                sfxFire.Play();
            }

            float con = rate.constant;
			if(transform.parent.tag == "Phone"){
				FireSpread(4.1f, con);
			}
			else if(transform.parent.tag == "Bin"){
				FireSpread(4f, con);
			}
			else if(transform.parent.tag == "Pan"){
				FireSpread(2.8f, con);
			}
		}
	}
	
	void FireSpread(float r, float con){
		if (con < 500.0f || rad.radius < r) {
			con += 10.0f;
			rate.constant = con;
			em.rate = rate;
			rad.radius += 0.005f;
			
            // increase the range of the where collision can be detected
			BoxCollider boxCollider = GetComponent<BoxCollider>();
			Vector3 colliderSize = boxCollider.size;
			colliderSize.x = rad.radius * 5.0f;
			colliderSize.y = rad.radius * 5.0f;
			boxCollider.size = colliderSize;
		}
        else { 
        }
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds(10f);
	}
}