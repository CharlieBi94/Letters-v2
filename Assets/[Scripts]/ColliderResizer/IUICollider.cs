using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for UI elements that need to be resized for the collider
/// </summary>
public interface IUICollider
{
    event Action SizeChanged;
    void InvokeSizeChanged();

}
