class_name CS_ChangeScene extends CutsceneEvent

@export var to_scene: PackedScene

func trigger(_caller: CutsceneSequence):
	_caller.get_tree().change_scene_to_packed(to_scene)
