class_name CS_ChangeScene extends CutsceneEvent

@export var to_scene: String

func trigger(_caller: CutsceneSequence):
	var _scene_res = load(to_scene)
	GameSystem.instance.change_game_world(_scene_res)
