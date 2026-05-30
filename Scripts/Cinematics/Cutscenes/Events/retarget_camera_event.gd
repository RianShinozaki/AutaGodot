class_name CS_RetargetCamera extends CutsceneEvent

@export var target_1: NodePath

func trigger(_caller):
	GameCamera.instance.retarget(_caller.get_node(target_1))
