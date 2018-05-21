using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class animations : MonoBehaviour {
    private Animator animator;
    private int level = 0;
	private Dictionary<string, AudioSource> instructorAudio = new Dictionary<string, AudioSource>();
		
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources) {
            instructorAudio.Add(source.clip.name, source);
        }
    }
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo currentAnim = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnim.IsName("Base Layer.start_1")) {
            playSound("1_introduction_part1v2", true);
        }
        else if (currentAnim.IsName("Base Layer.Walk")) {
            playSound("2_introduction_part2", true);
        }
        else if (currentAnim.IsName("Base Layer.idle2")) {
            playSound("3_instructions_part1", true);
        }
        else if (currentAnim.IsName("Base Layer.Point")) {
            playSound("4_instructions_part2", true);
        }
        else if (currentAnim.IsName("Base Layer.Stand")) {
            playSound("5_instructions_part2.5_endv2", true);
        }
        else if (currentAnim.IsName("Base Layer.180turn 0")) {
            playSound("10_thatbin_burningcard", true);
        }
        else if (currentAnim.IsName("Base Layer.end_1")) {
			playParticle("SmallTable (1)/smartphone_n4h/Particle System (1)");
			StartCoroutine (SmokeWait ("SmallTable (1)/smartphone_n4h/WhiteSmoke", "SmallTable (1)/smartphone_n4h/Particle System (2)"));
            playSound("9_note7_haywire", true);
        }
        else if (currentAnim.IsName("Base Layer.end_2")) {
			playParticle("RoundRubbishBin/WhiteSmoke");
			StartCoroutine(FireWait("RoundRubbishBin/Particle System (1)"));
        }
        else if (currentAnim.IsName("Base Layer.end_3")) {
            playSound("8_ohnoes_the_hob", true);
			playParticle("Sink.1/pan_2/Particle System");
			StartCoroutine(SmokeWait("Sink.1/pan_2/WhiteSmoke", "Sink.1/pan_2/Particle System (1)"));
        }
    }

    private void playParticle(string objPath) {
        Transform obj = transform.parent.Find(objPath);
        ParticleSystem particle = obj.GetComponent<ParticleSystem>();
        if (!obj.gameObject.activeSelf || !particle.isPlaying) {
            obj.gameObject.SetActive(true);
            obj.GetComponent<ParticleSystem>().Play();
        }
    }

    public void playSound(string audioName, bool removeAfter) {
        if (instructorAudio.ContainsKey(audioName)) { 
            if (!instructorAudio[audioName].isPlaying) { 
                instructorAudio[audioName].Play();

                if (removeAfter) {
                    instructorAudio.Remove(audioName);
                }
            }
        }
    }
	
	IEnumerator SmokeWait(string smokeName, string fireName){
		yield return new WaitForSeconds(3f);
		playParticle(smokeName);
		StartCoroutine (FireWait (fireName));
	}
		
	IEnumerator FireWait(string fireName){
		yield return new WaitForSeconds(3f);
		Transform fire = transform.parent.Find(fireName);
		fire.GetComponent<FireSpreader>().soldierFree = gameObject;
		fire.GetComponent<ParticleSystem>().Play();
	}

	public void startGame() {
		
		level += 1;	
		begin(level);
	}
	
	public void startGame(bool started) {
		
		if(!started){
			level += 1;
		}
		
		begin(level);
	}
	
	public void begin(int level){
		Debug.Log("starting level: " + level);
		if (level <= 3) {
			animator.Play("start_" + level);
		}
		else if (level == 4) {
			Transform endGameText = transform.parent.Find("End Game");
			endGameText.localPosition = new Vector3(-60, 38, 11);
			endGameText.localScale = new Vector3(12, 8, 1);
		}
	}
	
	
	
}
