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

func on_body_entered(body):
	if not active:
		var pos = body.global_position
		
		var dist_left = abs(pos.x - x_extents.x)
		var dist_right = abs(pos.x - x_extents.y)
		var dist_top = abs(pos.y - y_extents.x)
		var dist_bottom =abs(pos.y - y_extents.y)
		
		var min_dist = min(dist_left, dist_right, dist_top, dist_bottom)
		
		if abs(min_dist) < 64:
			if min_dist == dist_left:
				while(body.global_position.x < x_extents.x+16):
					body.global_position.x+=1
					
			elif min_dist == dist_right:
				while(body.global_position.x > x_extents.y-16):
					body.global_position.x-=1
					
			elif min_dist == dist_top:
				while(body.global_position.y < y_extents.x+16):
					body.global_position.y+=1
					
			else:
				while(body.global_position.y > y_extents.y-16):
					body.global_position.y-=1
				body.velocity.y = -250
			
		var _crs = get_tree().get_nodes_in_group("CamRegion")
		for _cr in _crs:
			_cr.deactivate_self()
		active = true
		GameCamera.instance.set_extents(x_extents, y_extents)
		GameCamera.instance.snap_to_target()
		emit_signal("activate")
	
func deactivate_self():
	active = false
	emit_signal("deactivate")

func set_extents() -> void:
	var _total_width: float = $CollisionShape2D.shape.size.x * global_scale.x
	var _total_height: float = $CollisionShape2D.shape.size.y * global_scale.y
	x_extents = Vector2($CollisionShape2D.global_position.x - _total_width/2, $CollisionShape2D.global_position.x + _total_width/2) 
	y_extents = Vector2($CollisionShape2D.global_position.y - _total_height/2, $CollisionShape2D.global_position.y + _total_height/2) 
