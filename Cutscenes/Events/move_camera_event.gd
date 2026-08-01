class_name CS_MoveCamera
extends CutsceneEvent

@export var to_position: Vector2
@export var to_object: NodePath
@export var duration: float
@export var be_async: bool

func trigger(_caller: CutsceneSequence):
	async = be_async
	var _cam: GameCamera = GameCamera.instance
	if to_object != null:
		to_position = _caller.get_node(to_object).global_position
	_cam.lerp_to_position(to_position, duration)
	await _cam.reached_position
	emit_signal("finished")
