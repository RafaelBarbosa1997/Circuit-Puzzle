# Circuit-Puzzle
 A customizable circuit puzzle creator for the Unity Engine.
 
## Instalation instructions
Simply go to the latest release and download the Unity package then import it into your Unity project.

## Usage instructions
### Instantiation
Drag either the basic circuit puzzle or the advanced circuit puzzle prefab onto your scene.

You will be presented with a warning as creating a circuit puzzle instance will clear your undo history.

The advanced prefab comes with a camera that will be switched to when beggining interaction with the puzzle. Ending interaction will switch back to the previously active camera. The interact point also has a visible mesh while it is invisible in the basic version.

Apart from that, both versions are the same.

### Puzzle board creation
**To create the puzzle board**, simply select the desired number of row and columns in the inspector, either by using the arrows or typing in the value, and press apply. Pressing cancel will reset values to previously applied values.

Green squares will appear to preview the board's appearance. Red squares will also appear when deleting row or columns on an existing board.

**Clear board option** deletes the whole puzzle.

**The limiter option** simply limits the max value that the row and columns input takes, to avoid crashing from an overly high value. **Disable at own risk**.

### Designing the puzzle
**To design your desired puzzle**, simply click each individual piece.

**To change a piece's type**, simply change it in the inspector while the desired piece is selected.

**To rotate each piece**, use the arrows in the piece rotator component while the piece is selected.

### Piece types
**The start piece** type is where power is generated. Pieces connected to it will become powered on.

**The straight, corner and T piece** are connector pieces, and are used to pass power through each other and to other pieces.

**End pieces** will run user defined events when powered on or off..

**Blank pieces are just used to fill out empty spaces on the board and have no functionality.

### Setting up end events
**Single and grouped mode** define how puzzle completion and therefore events are triggered.

**Grouped mode** uses the grouped ending events component, in the main circuit puzzle object.

In this mode, OnPoweredOn events will run when every ending piece becomes powered and OnPoweredOff will run if any ending piece is powered off while all were previously powered.

**Single mode** uses the ending piece events component inside each individual ending piece.

In this mode, OnPoweredOn events will run when that specific piece is powered and OnPoweredOff will run if that specific piece is powered off.

### Interacting with the puzzle
**To interact with the puzzle**, create a script that will access a puzzle instance's **interact point component**, located in the **Interact Point child** of the main puzzle Gameobject.

**BeginInteraction() method** must be called to begin interaction with the puzzle.

After that, you can control the puzzle using the following methods:

///

**RotatePieceLeft()**

**RotatePieceRight()**

**MoveSelectionVertical() ->** send -1 as parameter to move down and 1 to move up

**MoveSelectionHorizontal() ->** send -1 as parameter to move left and 1 to move right

///

**EndInteraction() method** must be called when ending interaction with the puzzle.

Also in the **Interact Point component**, in the inspector, you may **set up events to run when beggining and ending the interaction**.

### Puzzle settings
The puzzle type can be set to **continuous or one time**:

**In continuous mode**, the user can continue interacting with the puzzle indefinitely.

**In one time mode**, once power has been turned on, EndInteraction will run and user will no longer be able to interact with the puzzle.

///

**Starter and ending pieces** can be set to **locked or unlocked**:

**If pieces are locked**, they can not be rotated by the player.

**If pieces are unlocked**, they can be rotated by the player.

### Custom models
You can use your own models for the puzzle pieces.

In the main circuit puzzle GameObject, go to the user models component and assign a prefab of your model to each corresponding entry.

After you have assigned every model, enable custom models in user model selection component. You will only be able to enable if every model has been properly assigned.

**IMPORTANT:** custom models X and Y size values must be 1 and 1 in Unity units, or pieces will not be properly aligned.
