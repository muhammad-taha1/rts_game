using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public record ButtonDefinition(string displayName, string imagePath, Action action)
{
    public string displayName { get; private set; } = displayName;
    public string imagePath { get; private set; } = imagePath;
    public Action action { get; private set; } = action;


}
