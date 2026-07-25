using UnityEngine;

[CreateAssetMenu(
    fileName = "New Crew Profile",
    menuName = "AV Game/Crew Profile"
)]
public class SO_CrewProfile : ScriptableObject
{
    [Header("Identity")]
    public string crewName;

    public string role;

    public int level = 1;


    [Header("Skills")]
    [Range(0, 5)]
    public int riggingSkill;

    [Range(0, 5)]
    public int audioSkill;

    [Range(0, 5)]
    public int lightingSkill;

    [Range(0, 5)]
    public int videoSkill;


    [Header("Progression")]
    public int experience;
}