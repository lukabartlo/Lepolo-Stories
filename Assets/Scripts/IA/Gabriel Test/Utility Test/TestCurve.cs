using UnityEngine;

[CreateAssetMenu(
    fileName = "TestCurve",
    menuName = "Custom/Test Curve",
    order = 0)]
public class TestCurve : ScriptableObject
{
    [SerializeField] AnimationCurve PriorityCurve;
    public bool boolTest = true;
    
    public float TestPriorityLevel(float min, float max, float value)
    {
        return PriorityCurve.Evaluate(Mathf.InverseLerp(min , max, value)) * System.Convert.ToInt32(boolTest);
    }
}
