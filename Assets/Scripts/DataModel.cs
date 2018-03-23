using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DM {

    public class Model:MonoBehaviour
    {
		public static int AIeffectiveAttack = 1;
		public static int AIeffectiveDefense = 1;
		public static int PeffectiveAttack = 1;
		public static int PeffectiveDefense = 1;
		public static int InputAttack = 1;
		public static float RunAway = 0;
		
		//Initial Hann window size
		// The weight of nth element: w(n) = sin2(3.14 * n / (N - 1))

		private static float x1 = 0;
		private static float x2 = 0;
		private static float x3 = 0;
		private static float x4 = 0;
		
		private static List<Vector4> StructuredData;
		
		private const int AGILE = 0;
		private const int AGGRESSIVE = 1;
		private const int CONSERVATIVE = 2;
		private const int PASSIVE = 3;
		
        public static int DataProcess()
        {
            x1 = PeffectiveAttack / InputAttack;
			x2 = RunAway;
			x3 = PeffectiveAttack / PeffectiveDefense - 1;
			x4 = AIeffectiveAttack / AIeffectiveDefense - 1;
			
			Vector4 Features = new Vector4(x1, x2, x3, x4);
			StructuredData.Add(Features);
			//
			
			//Convolution process
			
			float Y = 0.8f * x1 + 0.2f * x2;
			float X = x3 - x4;
			
			print("X:" + X + " Y:" + Y);

			if (Y >= 0 && X >= 0) {
				return AGGRESSIVE;
			} else if (Y <= 0 && X >= 0) {
				return PASSIVE;
			} else if (Y >= 0 && X <= 0) {
				return AGILE;
			} else if (Y <= 0 && X <= 0) {
				return CONSERVATIVE;
			}
			
			return -1;
        }

       
    }
}
