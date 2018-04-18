using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace DM {

    public class Model:MonoBehaviour
    {
		public static int AIeffectiveAttack = 1;
		public static int AIeffectiveDefense = 1;
		public static int PeffectiveAttack = 1;
		public static int PeffectiveDefense = 1;
		public static int InputAttack = 1;
		public static float RunAway = 0;
		
		/////////////////////////////////////////////////////////////////////////////////
		// Initial Hamming window size												   //
		// The weight of nth element: w(n) = 1 - 0.5 * cos(2 * 3.14 * n / M - 1)	   //
		// This is used for the convolutional window. This is to consider the delay.   // 
		// M = WindowSize															   //
		/////////////////////////////////////////////////////////////////////////////////
		
		private static int WindowSize = 5;

		private static float x1 = 0;
		private static float x2 = 0;
		private static float x3 = 0;
		private static float x4 = 0;
		
		private static List<Vector4> StructuredData = new List<Vector4>();
		private static List<Vector4> ProcessedData = new List<Vector4>();
		private static List<Vector2> FeatureVectors = new List<Vector2>();
		
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
			
			AIeffectiveAttack = 1;
			AIeffectiveDefense = 1;
			PeffectiveAttack = 1;
			PeffectiveDefense = 1;
			InputAttack = 1;
			RunAway = 0;
		
			
			//Preprocess
			//Hann function process
			
			if (StructuredData.Count > 10) {
				StructuredData.RemoveAt(0);
			}
			
			if (StructuredData.Count > 4 && StructuredData.Count < 11) {
				int i = 0;
			
				for (i = 5; i > 0; i--) {
				
					float weight = 1 - 0.5f * (float)Math.Cos(2f * 3.14f * i / (WindowSize - 1));
					//The minimum weight inside the window is set as 0.5
					
					ProcessedData.Add(weight * StructuredData[StructuredData.Count - i]);
				}
				
				if (StructuredData.Count > 5) {
				
					for (i = 0; i < StructuredData.Count - 5; i++) {
						ProcessedData.Add(0.3f * StructuredData[i]);
					}

					//The weight outside the window is all 0.3
				}
			} else {
				return 0;
			}
			
			
//			foreach (Vector4 v in ProcessedData) {
//				print(v);
//			}
			
			
			//Convolution process
			//Convolution function: Y = 0.8F * X1 + 0.2F * X2   X = X3 - X4
			
			float X = 0;
			float Y = 0;
			foreach (Vector4 v in ProcessedData) {
			
				X =  0.8f * v.x - 0.2f * v.y;
				Y = v.z - 0.7f * v.w;
				
				FeatureVectors.Add(new Vector2(X, Y));
			}
			
			X = 0;
			Y = 0;
			
			foreach (Vector2 v in FeatureVectors) {
				X += v.x;
				Y += v.y;
			}
			ProcessedData.Clear();
			FeatureVectors.Clear();
			
			if (Y >= 0 && X >= 0) {
				return AGGRESSIVE;
			} else if (Y <= 0 && X >= 0) {
				return PASSIVE;
			} else if (Y <= 0 && X <= 0) {
				return AGILE;
			} else if (Y >= 0 && X <= 0) {
				return CONSERVATIVE;
			}
			
			
			
			return AGILE;
        }

       
    }
}
