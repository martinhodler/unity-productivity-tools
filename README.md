# Unity Productivity Tools
Extends the Unity Editor with simple productivity tools like an easy way to serialize SceneAssets in your component, grouping GameObjects in the scene and more.

- [Scene View / Hierarchy](#scene-view--hierarchy)
  - [Grouping](#grouping)
- [Attributes](#attributes)
  - [Button](#button)
  - [Help](#help)
  - [MinMaxRange](#minmaxrange)
  - [NavMeshAreaMask](#navmeshareamask)
  - [Scene](#scene)
  - [Tag](#tag)




## Scene View / Hierarchy
### Grouping
By selecting more than one GameObject in the Scene or in the Hierarchy you can group the selected objects by clicking on **Edit > Group** or by pressing <kbd>Ctrl</kbd> + <kbd>G</kbd>.


## Attributes
Unity allows you to use Attributes on your properties and fields. UPT extends the set of Attributes with these custom attributes:

### Button 
`[Button (ButtonAvailability availability) ]`

Displays a button at the top of the inspector which invokes the assigned Method.<br>Methods with params will be displayed as a foldout.
<br>Limited to methods.
<br>**Example:**<br>
```c#
[Button(ButtonAvailability.Play)]
private void MyTestMethod(int testValue)
{
    ...
}
```


### Help
`[Help (string message, MessageType type = MessageType.Info) ]`

Displays a HelpBox above any property.
<br>**Example:**<br>
```c#
[Help("Little description on how you should set this variable")]
public Vector3 spawnPosition;
````

### MinMaxRange
`[MinMaxRange (float min, float max, float stepSize = 1f) ]`

Displays the property as a slider with a min and max value to choose from.
<br>Limited to `float`.
<br>**Example:**<br>
```c#
[MinMaxRange(0, 100, 1)]
public Vector2 spawnDelayRange;
```

### NavMeshAreaMask
`[NavMeshAreaMask]`

Displays the property as a Mask-Selection for NavMeshAreas similar to LayerMask for Layers.
<br>Limited to `int`.
<br>**Example:**<br>
```c#
[NavMeshAreaMask]
public int walkableMask;
```

### Scene
`[Scene (inBuildCheck = true) ]`

Displays the property as a SceneAsset which allows to drag & drop or select Scenes from the project folder.
<br>Limited to `string`.
<br>**Example:**<br>
```c#
[Scene]
public string menuScene;
```


### Tag
`[Tag]`

Displays the property as a dropdown with all the defined Tags.
<br>Limited to `string`.
<br>**Example:**<br>
```c#
[Tag]
public string teamTag;
```

# License
See [LICENSE.md](LICENSE.md)
