class_name CamRegion

extends Area2D

var active: bool = false

var x_extents: Vector2
var y_extents: Vector2

signal activate
signal deactivate

func _ready() -> void:
	body_entered.connect(on_body_entered)
	set_extents()
	
func on_body_entered(_body: Node2D):
	if not active:
		var _crs = get_tree().get_nodes_in_group("CamRegion")
		for _cr in _crs:
			_cr.deactivate_self()
		active = true
		GameCamera.instance.set_extents(x_extents, y_extents)
		emit_signal("activate")
	
func deactivate_self():
	active = false
	emit_signal("deactivate")

func set_extents() -> void:
	var _total_width: float = $CollisionShape2D.shape.size.x * global_scale.x
	var _total_height: float = $CollisionShape2D.shape.size.y * global_scale.y
	x_extents = Vector2(global_position.x, global_position.x + _total_width) 
	y_extents = Vector2(global_position.y, global_position.y + _total_height) 
