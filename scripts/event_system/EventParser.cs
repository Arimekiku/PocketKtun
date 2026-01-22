using System;
using System.Collections.Generic;
using Godot;
using Scripts.Gameplay.Messages;

namespace Scripts.EventSystem;

public class EventParser
{
    public Event[] ParseEventSheet(string dataPath)
    {
        var events = new List<Event>();
        
        var file = FileAccess.Open(dataPath, FileAccess.ModeFlags.Read);
        var fileContent = file.GetAsText();

        var json = Json.ParseString(fileContent);
        var dictionary = json.AsGodotDictionary();
        foreach (var (eventName, eventData) in dictionary)
        {
            var data = eventData.AsGodotDictionary();
            var messages = data["Messages"].AsGodotArray();
            
            var instance = new Event();
            instance.EventId = eventName.AsString();
            foreach (var value in messages)
            {
                var msg = value.AsGodotDictionary();
                var stringType = msg["Type"].AsString();
                var enumType = ParseEnum<GameMessages>(stringType);

                var boolValue = msg.GetValueOrDefault("Bool", false).AsBool();
                var intValue = msg.GetValueOrDefault("Int", 0).AsInt32();
                var floatValue = msg.GetValueOrDefault("Float", 0.0f).AsSingle();
                
                var processedData = new GameMessage(enumType)
                {
                    Bool = boolValue,
                    Int = intValue,
                    Float = floatValue,
                };
                instance.AddProcessedData(processedData);
            }
            events.Add(instance);
        }
        
        return events.ToArray();
    }
    
    private T ParseEnum<T>(string value) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), value);
    }
}