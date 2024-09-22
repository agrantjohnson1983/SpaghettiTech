using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenuAttribute(fileName = "SO_EventsUI", menuName = "ScriptableObjects/UI")]
public class SO_EventsUI : ScriptableObject
{

    public UnityEvent<string> instructionsRiggingEvent, instructionsAudioEvent, instructionsVideoEvent, instructionsLightingEvent;

    public UnityEvent<float> taskTimeEvent;

    public UnityEvent<SO_CharacterData> characterChangeEvent;

    public UnityEvent<Sprite, int[]> itemHeldImage;

    public UnityEvent<bool> motorControlDisplay;

    public UnityEvent<bool> motorControlTrigger;
    public UnityEvent motorControlStop;

    public UnityEvent<string> controlsPopup;

    public UnityEvent<float> characterHire;

    public UnityEvent<SO_ItemData> toolHeldImage;

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
            taskTimeEvent = new UnityEvent<float>();
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
            controlsPopup = new UnityEvent<string>();
        }

        if(characterHire == null)
        {
            characterHire = new UnityEvent<float>();
        }

        if(toolHeldImage == null)
        {
            toolHeldImage = new UnityEvent<SO_ItemData>();
        }
    }

    // Used for triggering and changing the instructions
    public void InstructionsRiggingTrigger(string _instructions)
    {
        instructionsRiggingEvent.Invoke(_instructions);
    }

    public void InstructionsAudioTrigger(string _instructions)
    {
        instructionsAudioEvent.Invoke(_instructions);
    }

    public void InstructionsVideoTrigger(string _instructions)
    {
        instructionsVideoEvent.Invoke(_instructions);
    }

    public void InstructionsLightingTrigger(string _instructions)
    {
        instructionsLightingEvent.Invoke(_instructions);
    }

    public void TriggerTaskGauge(float _time)
    {
        taskTimeEvent.Invoke(_time);
    }

    public void TriggerCharacterChange(SO_CharacterData _characterData)
    {
        characterChangeEvent.Invoke(_characterData);
    }

    public void TriggerItemHeldImage(Sprite _sprite, int[] _indexArray)
    {
        itemHeldImage.Invoke(_sprite, _indexArray);
    }

    public void ToggleMotorControlDisplay(bool _controllerOn)
    {
        motorControlDisplay.Invoke(_controllerOn);
    }

    public void TriggerMotorControls(bool _isUp)
    {
        motorControlTrigger.Invoke(_isUp);
    }

    public void TriggerMotorControlStop()
    {
        motorControlStop.Invoke();
    }

    public void ToggleControlsPopup(string _popupText)
    {
        controlsPopup.Invoke(_popupText);
    }

    public void TriggerCharacterHire(float _cost)
    {
        characterHire.Invoke(_cost);
    }

    public void TriggerToolChange(SO_ItemData _itemData)
    {
        toolHeldImage.Invoke(_itemData);
    }
}
