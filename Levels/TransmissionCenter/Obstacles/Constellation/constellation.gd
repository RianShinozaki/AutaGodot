class_name Constellation
extends Area2D

static var last_touched_constellation: Constellation

@export var star_trail: PackedScene
@export var ripple: PackedScene
@export var camera_target: Node2D

var pulling: bool = false
var connected_constellations: Array[Constellation]

# Called when the node enters the scene tree for the first time.
func _ready():
	body_entered.connect(on_body_entered)


func on_body_entered(_body: Node):
	if not _body is Auta: return
	var _auta: Auta = _body as Auta
	_auta.global_position = global_position
	_auta.switch_action_state_name("ConstellationState")
	if last_touched_constellation and last_touched_constellation != self and not last_touched_constellation in connected_constellations:
		connected_constellations.append(last_touched_constellation)
		last_touched_constellation.connected_constellations.append(self)
		var _star_trail = star_trail.instantiate()
		get_parent().add_child(_star_trail)
		_star_trail.global_position = (last_touched_constellation.global_position + global_position)/2
		_star_trail.emission_rect_extents.x = (_star_trail.global_position - last_touched_constellation.global_position).length()
		_star_trail.rotation = (_star_trail.global_position - last_touched_constellation.global_position).angle()
	last_touched_constellation = self
	var _ripple = ripple.instantiate()
	add_child(_ripple)
	_ripple.global_position = global_position
	if camera_target:
		GameCamera.instance.retarget_multiple([Auta.instance, camera_target])

func _physics_process(delta):
	if pulling:
		var _auta: Auta = Auta.instance
		var _dir = (global_position - _auta.global_position).normalized()
		if _dir.dot(_auta.velocity.normalized()) > -0.8:
			_auta.velocity = _auta.velocity.move_toward(250*_dir, 700 * delta)


func _on_pull_box_area_entered(area):
	pulling = true


func _on_pull_box_area_exited(area):
	pulling = false
