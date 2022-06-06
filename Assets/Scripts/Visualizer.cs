using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Visualizer : MonoBehaviour
{
    private static System.Collections.Generic.List<Visualizer> m_visuals = new System.Collections.Generic.List<Visualizer>();
    private Text text;

    private void Awake()
    {
        m_visuals.Add(this);
        text = GetComponent<Text>();
    }

    public static void UpdateVisualizerData( string visualizerName , string newData )
    {
        foreach( Visualizer visual in m_visuals )
        {
            if( visual.name == visualizerName )
            {
                visual.text.text = newData;
                return;
            }
        }
    }
}