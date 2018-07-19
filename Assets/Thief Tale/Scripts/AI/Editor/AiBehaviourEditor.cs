using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.IO;

public class AiBehaviourEditor : MonoBehaviour {


    [MenuItem("Assets/Create/ThiefTale/AI/AiBehaviour")]
    public static void Create()
    {
        AnimatorController animatorController = new AnimatorController();

        //Get the current active folder
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!Directory.Exists(path))
        {
            Debug.LogAssertion("Please create AiBehaviour from project window");
            return;
        }
        path += "/AiBehaviour.asset";

        AssetDatabase.CreateAsset(animatorController, path);
        animatorController.AddParameter("IsPlayerInSight", AnimatorControllerParameterType.Bool);
        animatorController.AddParameter("IsIdle", AnimatorControllerParameterType.Bool);
        animatorController.AddParameter("SqrDistanceToPlayer", AnimatorControllerParameterType.Float);
        animatorController.AddParameter("HeardNoise", AnimatorControllerParameterType.Bool);

        animatorController.AddLayer("Base Layer");

        AssetDatabase.SaveAssets();
    }
}
