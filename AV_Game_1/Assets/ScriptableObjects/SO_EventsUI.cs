using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenuAttribute(fileName = "SO_EventsUI", menuName = "ScriptableObjects/UI")]
public class SO_EventsUI : ScriptableObject
{

    public UnityEvent<string> instructionsRiggingEvent, instructionsAudioEvent, instructionsVideoEvent, instructionsLightingEvent;

    public UnityEvent<float, Vector3> taskTimeEvent;

    public UnityEvent<SO_CharacterData> characterChangeEvent;

    public UnityEvent<Sprite, int[]> itemHeldImage;

    public UnityEvent<bool> motorControlDisplay;

    public UnityEvent<bool> motorControlTrigger;
    public UnityEvent motorControlStop;

    public UnityEvent<string,string> controlsPopup;

    public UnityEvent<SO_CrewProfile> crewHire;

    public UnityEvent<SO_ToolData> toolHeldImage;

    public UnityEvent<string, float> messageEvent;

    public UnityEvent<float> progOverall, progRigging, progAudio, progVideo, progLighting;

    public UnityEvent nutPickup, boltPickup;

    private void OnEnable()
    {
        if(instructionsRiggingEvent == null)
        {
            instructionsRiggingEvent = new UnityEvent<string>();
        }

        if (instructionsAudioEvent == null)
        {
            instructionsAudioEvent = new UnityEvent<string>();
        }

        if (instructionsVideoEvent == null)
        {
            instructionsVideoEvent = new UnityEvent<string>();
        }

        if (instructionsLightingEvent == null)
        {
            instructionsLightingEvent = new UnityEvent<string>();
        }

        if (taskTimeEvent == null)
        {
            taskTimeEvent = new UnityEvent<float, Vector3>();
        }

        if(characterChangeEvent == null)
        {
            characterChangeEvent = new UnityEvent<SO_CharacterData>();
        }
        
        if(itemHeldImage == null)
        {
            itemHeldImage = new UnityEvent<Sprite, int[]>();
        }

        if(motorControlDisplay == null)
        {
            motorControlDisplay = new UnityEvent<bool>();
        }

        if(motorControlTrigger == null)
        {
            motorControlTrigger = new UnityEvent<bool>();
        }

        if(motorControlStop == null)
        {
            motorControlStop = new UnityEvent();
        }

        if(controlsPopup == null)
        {
            controlsPopup = new UnityEvent<string, string>();
        }

        if(crewHire == null)
        {
            crewHire = new UnityEvent<SO_CrewProfile>();
        }

        if(toolHeldImage == null)
        {
            toolHeldImage = new UnityEvent<SO_ToolData>();
        }

        if (messageEvent == null)
        {
            messageEvent = new UnityEvent<string, float>();
        }

        if(progOverall == null)
        {
            progOverall = new UnityEvent<float>();
        }

        if (progRigging == null)
        {
            progRigging = new UnityEvent<float>();
        }

        if (progAudio == null)
        {
            progAudio = new UnityEvent<float>();
        }

        if (progVideo == null)
        {
            progVideo = new UnityEvent<float>();
        }

        if (progLighting == null)
        {
            progLighting = new UnityEvent<float>();
        }

        if (nutPickup == null)
        {
            nutPickup = new UnityEvent();
        }

        if (boltPickup == null)
        {
            boltPickup = new UnityEvent();
        }

    }

    // Used for triggering and changing the instructions
    public void InstructionsRiggingTrigger(string _instructions)
    {
        instructionsRiggingEvent?.Invoke(_instructions);
    }

    public void InstructionsAudioTrigger(string _instructions)
    {
        instructionsAudioEvent?.Invoke(_instructions);
    }

    public void InstructionsVideoTrigger(string _instructions)
    {
        instructionsVideoEvent?.Invoke(_instructions);
    }

    public void InstructionsLightingTrigger(string _instructions)
    {
        instructionsLightingEvent?.Invoke(_instructions);
    }

    public void TriggerTaskGauge(float _time, Vector3 _pos)
    {
        taskTimeEvent?.Invoke(_time, _pos);
    }

    public void TriggerCharacterChange(SO_CharacterData _characterData)
    {
        characterChangeEvent?.Invoke(_characterData);
    }

    public void TriggerItemHeldImage(Sprite _sprite, int[] _indexArray)
    {
        itemHeldImage?.Invoke(_sprite, _indexArray);
    }

    public void ToggleMotorControlDisplay(bool _controllerOn)
    {
        motorControlDisplay?.Invoke(_controllerOn);
    }

    public void TriggerMotorControls(bool _isUp)
    {
        motorControlTrigger?.Invoke(_isUp);
    }

    public void TriggerMotorControlStop()
    {
        motorControlStop?.Invoke();
    }

    public void TriggerControlsPopup(string _controlsText, string _actionText)
    {
        controlsPopup?.Invoke(_controlsText, _actionText);
    }

    public void TriggerCrewHire(SO_CrewProfile _crew)
    {
        crewHire?.Invoke(_crew);
    }

    public void TriggerToolChange(SO_ToolData _toolData)
    {
        toolHeldImage?.Invoke(_toolData);
    }

    public void TriggerMessage(string _message, float _time)
    {
        messageEvent?.Invoke(_message, _time);
    }

    public void TriggerProgOverall(float _amount)
    {
        progOverall?.Invoke(_amount);
    }

    public void TriggerProgRigging(float _amount)
    {
        progRigging?.Invoke(_amount);
    }

    public void TriggerProgAudio(float _amount)
    {
        progAudio?.Invoke(_amount);
    }

    public void TriggerProgVideo(float _amount)
    {
        progVideo?.Invoke(_amount);
    }

    public void TriggerProgLighting(float _amount)
    {
        progLighting?.Invoke(_amount);
    }

    public void TriggerBoltPickup()
    {
        boltPickup?.Invoke();
    }

    public void TriggerNutPickup()
    {
        nutPickup?.Invoke();
    }
}
