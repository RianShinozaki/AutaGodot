class_name CS_CastToShadowRealm extends CutsceneEvent

@export var objects_to_cast: Array[NodePath]

func trigger(_caller: CutsceneSequence):
	for node in objects_to_cast:
		_caller.get_node(node).global_position = Vector2(-10000, -10000)
