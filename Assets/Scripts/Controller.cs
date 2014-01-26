using UnityEngine;
using System;
using System.Collections.Generic;

public class Controller : MonoBehaviour 
{
  public AudioClip deathSound;
  public AudioClip enableSound;
  public Transform musicPlayer;

	List<UniMoveController> moves = new List<UniMoveController>();
  List<UniMoveController> gameMoves = new List<UniMoveController>();
  
  UniMoveController moveToFlash = null;

  bool isCyan = true;
  bool gameOver = false;

  float hue = 0.5f;
  float hueRate = 0.02f;
	
	void Start() {
		Time.maximumDeltaTime = 0.1f;
		
		int count = UniMoveController.GetNumConnected();
    Debug.Log("count: " + count);
		
		for (int i = 0; i < count; i++) {
			UniMoveController move = gameObject.AddComponent<UniMoveController>();

			if (!move.Init(i)) {	
				Destroy(move);
				continue;
			}
					
			PSMoveConnectionType conn = move.ConnectionType;
			if (conn == PSMoveConnectionType.Unknown || conn == PSMoveConnectionType.USB) {
				Destroy(move);
			}	else {
				moves.Add(move);
			}
		}
	}

	
	void Update() {
    if(!gameOver) {
      if(gameMoves.Count >= 2) {
        foreach (UniMoveController gameMove in gameMoves) {
          if (gameMove.Disconnected) continue;

          if(gameMove.AnyButtonDown()) {
            gameMove.SetLED(Color.red);
            gameMove.SetRumble(1);
            audio.PlayOneShot(deathSound);
            gameOver = true;
            musicPlayer.audio.Stop();
            foreach (UniMoveController otherMove in gameMoves) {
              if (otherMove != gameMove) moveToFlash = otherMove;
            }
          }
        }
      } else {
        foreach (UniMoveController move in moves) {
          if (move.Disconnected) continue;

          if (move.AnyButtonDown() && !gameMoves.Contains(move)) {
            audio.PlayOneShot(enableSound);
            gameMoves.Add(move);
            move.SetLED(isCyan ? Color.cyan : Color.magenta);
            isCyan = !isCyan;
          }
        }
      }
    } else {
      FlashMove();
      foreach (UniMoveController move in moves) {
        if(move.AnyButtonDown()) {
          Application.LoadLevel(0);
        }
      }
    }
	}

  void FlashMove() {
    if (moveToFlash == null) return;
    hue += hueRate;
    if(hue > 1) hue = 0;
    moveToFlash.SetLED(new HSBColor(hue, 0.5f, 0.7f).ToColor());
  }
}
