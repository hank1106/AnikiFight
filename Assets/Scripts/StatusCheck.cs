using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DM;

namespace SC {

    public class StatusCheck
    {
		public static int PgetHitType = 0;
		public static int AIgetHitType = 0;
		public static bool PlightningStatus = false;
		public static bool AIlightningStatus = false;
		
        private const float LIGHT_ATTACK = 2.5f;
		private const float HEAVY_ATTACK = 3f;
		private const int NONE = 0;
		private const int LIGHT = 1;
		private const int HEAVY = 2;
		private const int LIGHTNING = 3;
		
        public static int PositionCheck(float playerX, float playerY, float AIX, float AIY, Rigidbody2D rg2d)
        {
            float distance = (AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY);
			Model.RunAway = distance - Model.RunAway;
            if (distance < 4f)
            {
                return 1;
                // TODO: speed to 0
            } else if (distance > 9f && distance < 25f) {
				return 0;
			} else if (distance > 15f) {
				return 2;
			}

            return -1;

        }

        public static int AIBeingHitCheck()
        {
            float playerX = GameObject.Find("Aniki").transform.position.x;
            float playerY = GameObject.Find("Aniki").transform.position.y;
            float AIX = GameObject.Find("Enemy").transform.position.x;
            float AIY = GameObject.Find("Enemy").transform.position.y;

            float distance = Mathf.Sqrt((AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY));

            if (AIgetHitType == 1 && distance <= LIGHT_ATTACK)
            {
				GameControl.instance.Score ();
                return 1;
            } else if (AIgetHitType == 2 && distance <= HEAVY_ATTACK) {
				GameControl.instance.Score ();
				return 2;
			} else if (AIgetHitType == 3) {
				GameControl.instance.Score ();
				return 3;
			}
            return 0;
        }
		
		public static int PBeingHitCheck()
        {
            float playerX = GameObject.Find("Aniki").transform.position.x;
            float playerY = GameObject.Find("Aniki").transform.position.y;
            float AIX = GameObject.Find("Enemy").transform.position.x;
            float AIY = GameObject.Find("Enemy").transform.position.y;

            float distance = Mathf.Sqrt((AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY));

            if (PgetHitType == 1 && distance <= LIGHT_ATTACK)
            {
                return 1;
            } else if (PgetHitType == 2 && distance <= HEAVY_ATTACK) {
				return 2;
				
			}
            return 0;
        }

    }
}
