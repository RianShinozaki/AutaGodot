class_name CS_RetargetCamera extends CutsceneEvent

@export var target_1: NodePath
@export var ignore_boundaries: bool = false

func trigger(_caller):
	GameCamera.instance.retarget(_caller.get_node(target_1))
	GameCamera.instance.ignore_boundaries = ignore_boundaries
