using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(Dialogue_System))]
public class Custom_Dialogue_Editor : Editor
{
    Dialogue_System ds;

    private TextAsset
        m_textFile;

    private float
        m_addAmount;

    private bool[] m_showLines;

    private void OnEnable()
    {
        ds = (Dialogue_System)target;
        m_textFile = ds.m_textFile;
        UpdateLineCount();
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Text File:", GUILayout.Width(100));
        ds.m_textFile = (TextAsset)EditorGUILayout.ObjectField(ds.m_textFile, typeof(TextAsset), true);

        GUILayout.Label("Repeat at Line: (99 will deactivate dialog trigger)", GUILayout.Width(300));
        ds.m_repeatLine = EditorGUILayout.IntField(ds.m_repeatLine, GUILayout.Width(50));

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Auto Trigger:", GUILayout.Width(80));
        //ds.m_autoTrigger = EditorGUILayout.Toggle(ds.m_autoTrigger, GUILayout.Width(15));
        //GUILayout.EndHorizontal();

        //GUILayout.Label("Type Speed:", GUILayout.Width(100));
        //ds.m_typingSpeed = EditorGUILayout.FloatField(ds.m_typingSpeed, GUILayout.Width(50));


        if (GUILayout.Button("Refresh"))
        {
            ds = (Dialogue_System)target;
            m_textFile = ds.m_textFile;
            UpdateLineCount();
        }

        for (int i = 0; i < ds.m_textLines.Length; i++)
        {
            
            m_showLines[i] = EditorGUILayout.Foldout(m_showLines[i], "Line " + (i + 1).ToString() + ":" + ds.m_textLines[i]);

            if (m_showLines[i])
            {
                GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.Width(100));
                        //Displays the thumbnail
                        Texture2D myTexture = AssetPreview.GetAssetPreview(ds.m_changeList[i].m_chracter);
                        GUILayout.Label(myTexture);
                            GUILayout.BeginHorizontal();
                                //Assignment box for Character Image
                                GUILayout.Label("Image:", GUILayout.Width(40));
                                ds.m_changeList[i].m_chracter = (Sprite)EditorGUILayout.ObjectField(ds.m_changeList[i].m_chracter, typeof(Sprite), allowSceneObjects: true);
                            GUILayout.EndHorizontal();
                    GUILayout.EndVertical();


                    GUILayout.BeginVertical();
                            GUILayout.BeginHorizontal();
                                //Displays the character's Name 
                                GUILayout.Label("Name:", GUILayout.Width(40));
                                ds.m_changeList[i].m_name = GUILayout.TextField(ds.m_changeList[i].m_name, GUILayout.Width(50));
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                                //Displays the Right Side Bool
                                GUILayout.Label("Right:", GUILayout.Width(40));
                                ds.m_changeList[i].m_right = EditorGUILayout.Toggle(ds.m_changeList[i].m_right, GUILayout.Width(15));
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                                //Camera to cut too
                                GUILayout.Label("Camera:", GUILayout.Width(40));
                                ds.m_changeList[i].m_camera = (Camera)EditorGUILayout.ObjectField(ds.m_changeList[i].m_camera, typeof(Camera), true);
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                                //Vocal sound to play
                                GUILayout.Label("VOX:", GUILayout.Width(40));
                                ds.m_changeList[i].m_audio = (AudioClip)EditorGUILayout.ObjectField(ds.m_changeList[i].m_audio, typeof(AudioClip), true);
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                                //Call a Quest function
                                GUILayout.Label("Quest:", GUILayout.Width(40));
                                ds.m_changeList[i].m_questEventScript = (QuestEvent)EditorGUILayout.ObjectField(ds.m_changeList[i].m_questEventScript, typeof(QuestEvent), true);
                            GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                              
            }
        }

        //base.OnInspectorGUI();
    }

    void UpdateLineCount()
    {
        ds.m_textLines = (m_textFile.text.Split('\n'));
        m_addAmount = ds.m_textLines.Length - ds.m_changeList.Count;
        m_showLines = new bool[ds.m_changeList.Count];

        for (int i = 0; i < m_addAmount; i++)
        {
            ds.m_changeList.Add(new Dialogue_Box());
        }
        
    }
}
