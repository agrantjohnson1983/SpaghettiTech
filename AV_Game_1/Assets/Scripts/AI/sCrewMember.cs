using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class sCrewMember : MonoBehaviour
{
    //[Header("References")]
    //public GameObject selectionRing;

    [Header("Debug")]
    public CrewState state = CrewState.Idle;

    NavMeshAgent agent;

    [Header("Crew Profile")]
    public SO_CrewProfile profile;

    [Header("Crew Data")]
    public sCrewData crewData = new();

    public bool IsSelected { get; private set; }

    public CrewCommand currentCommand;

    public sCrewJob currentJob;

    float workTimer;
    public float workDuration = 3f;

    [Header("Event Channels")]
    public SO_JobCompletedEventChannel jobCompletedEvent;

    public bool IsPreviewed { get; private set; }

    public sCrewSelectionIndicator selectionIndicator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        //if (selectionRing != null)
        //    selectionRing.SetActive(false);
    }

    private void Start()
    {
        sCrewManager manager = FindObjectOfType<sCrewManager>();

        if (manager != null)
            manager.RegisterCrew(this);
    }

    private void OnDestroy()
    {
        sCrewManager manager = FindObjectOfType<sCrewManager>();

        if (manager != null)
            manager.UnregisterCrew(this);
    }

    void Update()
    {
        //Debug.Log("Current State is " + state);

        switch (state)
        {
            case CrewState.Moving:

                if (!agent.pathPending &&
                    agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (currentJob != null &&
                        currentJob.state == CrewJobState.Traveling)
                    {
                        ArriveAtJob();
                    }
                    else
                    {
                        state = CrewState.Idle;
                    }
                }


                if (currentJob != null &&
                   currentJob.state == CrewJobState.Working)
                {
                    Debug.Log("Working on job");
                    WorkOnJob();
                }

                break;

            case CrewState.Idle:
                LookForWork();
                break;

            case CrewState.Working:
                WorkOnJob();
                break;
        }
    }

    public void SetPreview(bool enabled)
    {
        IsPreviewed = enabled;


        if (enabled)
        {
            selectionIndicator.ShowPreview();
        }
        else
        {
            if (IsSelected)
                selectionIndicator.ShowSelected();
            else
                selectionIndicator.Hide();
        }
    }

    public void Select()
    {
        Debug.Log("Selecting crew: " + name);

        IsSelected = true;

        if (selectionIndicator != null)
            selectionIndicator.ShowSelected();
        else
        {
            Debug.LogWarning(name + " has no selection indicator assigned!");
        }
    }

    public void Deselect()
    {
        IsSelected = false;

        if (!IsPreviewed)
        {
            if (selectionIndicator != null)
                selectionIndicator.Hide();
        }
    }

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
        state = CrewState.Moving;
    }

    CrewJobType ConvertCommandToJob(CrewCommand command)
    {
        switch (command)
        {
            case CrewCommand.Rigging:
                return CrewJobType.BuildTruss;


            case CrewCommand.Power:
                return CrewJobType.ConnectPower;


            case CrewCommand.Loading:
                return CrewJobType.Load;
        }


        return CrewJobType.None;
    }

    public void AssignJob(sCrewJob job)
    {
        currentJob = job;

        Debug.Log(
            name +
            " assigned job: " +
            job.jobType +
            " Target: " +
            (job.target != null ? job.target.name : "No Target")
        );

        StartJob();
    }

    void LookForWork()
    {
        //Debug.Log($"{name}: Looking for work...");

        if (currentJob != null)
            return;


        CrewJobType desiredJob =
            ConvertCommandToJob(currentCommand);


        if (desiredJob == CrewJobType.None)
            return;


        sCrewJob job =
            sJobManager.instance.GetJob(desiredJob);

        if (job == null)
            Debug.Log("No job found.");
        else
            Debug.Log($"Found job: {job.jobType}");


        if (job != null)
        {
            AssignJob(job);
        }
    }

    void StartJob()
    {
        if (currentJob == null)
            return;

        Debug.Log(
            name +
            " traveling to job target: " +
            currentJob.target.name
        );


        MoveTo(currentJob.target.position);

        currentJob.state = CrewJobState.Traveling;
    }

    void ArriveAtJob()
    {
        Debug.Log(
            name +
            " arrived at job target!"
        );

        state = CrewState.Working;
        currentJob.state = CrewJobState.Working;
    }

    void WorkOnJob()
    {
        //Debug.Log("Work Work Work");

        workTimer += Time.deltaTime;

        if (workTimer >= workDuration)
        {
            Debug.Log("Completing job!");

            CompleteJob();
        }
    }

    void CompleteJob()
    {
        Debug.Log(
            name +
            " completed job: " +
            currentJob.jobType
        );


        state = CrewState.Idle;

        currentJob.state = CrewJobState.Complete;

        currentJob.completed = true;

        JobEventData data = new JobEventData
        {
            crewMember = this,
            job = currentJob
        };

        jobCompletedEvent.Raise(data);

        currentJob = null;

        currentCommand = CrewCommand.None;

        workTimer = 0f;

        GainExperience(10);
    }

    void GainExperience(int amount)
    {
        if (profile == null)
            return;

        profile.experience += amount;

        Debug.Log(
            profile.crewName +
            " gained " +
            amount +
            " XP"
        );
    }

    public void AssignCommand(CrewCommand command)
    {
        currentCommand = command;

        Debug.Log(
            gameObject.name +
            " assigned: " +
            command
        );
    }

    void OnDrawGizmos()
    {
        if (currentJob != null && currentJob.target != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(
                transform.position,
                currentJob.target.position
            );

            Gizmos.DrawSphere(
                currentJob.target.position,
                0.25f
            );
        }
    }
}