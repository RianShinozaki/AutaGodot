class_name GameCamera

extends Camera2D

@export var targets: Array[Node2D]
@export var target_override: bool = false
@export var position_override: bool = false
@export var target_position: Vector2
@export var ignore_boundaries: bool = false

static var instance: GameCamera

var x_extents: Vector2
var y_extents: Vector2

var shake_amount: float = 0

func _enter_tree() -> void:
	instance = self
	if not targets.is_empty():
		get_camera_target()
		global_position = target_position

func lerp_zoom(_target: int, _trans_time: float = 0.3) -> void:
	var _targf: float = float(_target)
	create_tween().tween_property(self, "zoom", Vector2.ONE * _targf, _trans_time).set_trans(Tween.TRANS_SINE)

func _physics_process(_delta: float) -> void:
	if !position_override:
		var _cs = get_camera_size()
		get_camera_target()
		global_position = global_position.lerp(target_position, 8 * _delta)
		if not ignore_boundaries:
			global_position.x = clamp(global_position.x, x_extents.x + _cs.x/2, x_extents.y - _cs.x/2)
			global_position.y = clamp(global_position.y, y_extents.x + _cs.y/2, y_extents.y - _cs.y/2)
		
	if shake_amount > 0:
		global_position += Vector2(randf_range(-shake_amount, shake_amount), randf_range(-shake_amount, shake_amount))

func get_camera_size():
	return get_viewport_rect().size/zoom.x
	
func get_camera_target() -> void:
	if !target_override:
		var _avg_pos: Vector2 = Vector2.ZERO
		for _target in targets:
			_avg_pos += _target.global_position
		target_position = _avg_pos / targets.size()

func set_extents(_x_extents: Vector2, _y_extents: Vector2):
	x_extents = _x_extents
	y_extents = _y_extents

func shake_screen(_amount: float, _duration: float):
	if shake_amount > _amount: return
	shake_amount = _amount
	await get_tree().create_timer(_duration).timeout
	shake_amount = 0

func override_target(_target_position: Vector2, _ignore_boundaries: bool = true):
	ignore_boundaries = _ignore_boundaries
	target_override = true
	target_position = _target_position

func unoverride_target():
	ignore_boundaries = false
	target_override = false
	
func snap_to_target():
	get_camera_target()
	global_position = target_position
	
func retarget(_target):
	targets.clear()
	targets.append(_target)
