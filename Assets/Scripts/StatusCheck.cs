using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DA;
using System;
using System.Globalization;

namespace SC {

    public class StatusCheck
    {

        private const float LIGHT_ATTACK = 2.5f;

        public static bool PositionCheck(float playerX, float playerY, float AIX, float AIY, Rigidbody2D rg2d)
        {
            float distance = (AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY);

            if (distance < 4f)
            {
                return true;
                // TODO: speed to 0
            }

            return false;

        }

        public static bool CheckStatus()
        {

            float playerX = GameObject.Find("Aniki").transform.position.x;
            float playerY = GameObject.Find("Aniki").transform.position.y;
            float AIX = GameObject.Find("Enemy").transform.position.x;
            float AIY = GameObject.Find("Enemy").transform.position.y;





            return false;
        }

        public static bool BeingHitCheck()
        {
            float playerX = GameObject.Find("Aniki").transform.position.x;
            float playerY = GameObject.Find("Aniki").transform.position.y;
            float AIX = GameObject.Find("Enemy").transform.position.x;
            float AIY = GameObject.Find("Enemy").transform.position.y;

            float distance = Mathf.Sqrt((AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY));

            if (Input.GetKeyDown(KeyCode.J) && distance <= LIGHT_ATTACK)
            {
                return true;
            }
            return false;
        }

        public static bool WithinAttackDistance()
        {
            float playerX = GameObject.Find("Aniki").transform.position.x;
            float playerY = GameObject.Find("Aniki").transform.position.y;
            float AIX = GameObject.Find("Enemy").transform.position.x;
            float AIY = GameObject.Find("Enemy").transform.position.y;

            float distance = Mathf.Sqrt((AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY));
            if (distance <= LIGHT_ATTACK)
            {
                return true;
            }
            return false;
        }

    }
}
