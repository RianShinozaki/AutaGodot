class_name GameCamera

extends Camera2D

@export var targets: Array[Node2D]
static var instance: GameCamera

func _enter_tree() -> void:
	instance = self
	if not targets.is_empty():
		global_position = get_camera_target()

func lerp_zoom(_target: int, _trans_time: float = 0.3) -> void:
	var _targf: float = float(_target)
	create_tween().tween_property(self, "zoom", Vector2.ONE * _targf, _trans_time).set_trans(Tween.TRANS_SINE)

func _physics_process(_delta: float) -> void:
	global_position = global_position.lerp(get_camera_target(), 4 * _delta)
	
func get_camera_target() -> Vector2:
	var _avg_pos: Vector2 = Vector2.ZERO
	for _target in targets:
		_avg_pos += _target.global_position
	return _avg_pos / targets.size()
