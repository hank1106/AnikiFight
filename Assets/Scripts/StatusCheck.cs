using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace SC {

    public class StatusCheck
    {

        private const float LIGHT_ATTACK = 2.5f;
		private const float HEAVY_ATTACK = 3f;
		
		public static int getHitType = 0;
		private const int NONE = 0;
		private const int LIGHT = 1;
		private const int HEAVY = 2;

        public static int PositionCheck(float playerX, float playerY, float AIX, float AIY, Rigidbody2D rg2d)
        {
            float distance = (AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY);

            if (distance < 4f)
            {
                return 1;
                // TODO: speed to 0
            } else if(distance > 4f && distance < 10f) {
				return 0;
			}

            return -1;

        }

        public static bool CheckStatus()
        {

            float playerX = GameObject.Find("Aniki").transform.position.x;
            float playerY = GameObject.Find("Aniki").transform.position.y;
            float AIX = GameObject.Find("Enemy").transform.position.x;
            float AIY = GameObject.Find("Enemy").transform.position.y;

            return false;
        }

        public static int BeingHitCheck()
        {
            float playerX = GameObject.Find("Aniki").transform.position.x;
            float playerY = GameObject.Find("Aniki").transform.position.y;
            float AIX = GameObject.Find("Enemy").transform.position.x;
            float AIY = GameObject.Find("Enemy").transform.position.y;

            float distance = Mathf.Sqrt((AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY));

            if (getHitType == 1 && distance <= LIGHT_ATTACK)
            {
                return 1;
            } else if (getHitType == 2 && distance <= HEAVY_ATTACK) {
				return 2;
				
			}
            return 0;
        }

    }
}
