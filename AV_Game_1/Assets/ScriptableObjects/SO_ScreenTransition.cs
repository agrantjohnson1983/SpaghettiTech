using UnityEngine;

[CreateAssetMenu(
    fileName = "ScreenTransition",
    menuName = "Screen Transition/Transition"
)]
public class SO_ScreenTransition : ScriptableObject
{
    [Header("Transition")]
    public Material material;

    [Min(0.01f)]
    public float duration = 0.75f;

    public AnimationCurve curve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Appearance")]
    public Color color = Color.black;
}