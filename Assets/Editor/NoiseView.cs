using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextureGenerator))]
public class NoiseView : Editor
{
   public override void OnInspectorGUI()
   {
       TextureGenerator noiseGen = (TextureGenerator) target;

       DrawDefaultInspector ();

      if(GUILayout.Button("Generate"))
      {
         noiseGen.StartFromEditor();
      }
   }
}
