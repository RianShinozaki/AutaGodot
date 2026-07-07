class_name CS_ChangeScene extends CutsceneEvent

@export var to_scene: PackedScene

func trigger(_caller: CutsceneSequence):
	GameSystem.instance.change_game_world(to_scene)
