class_name GravityEffector
extends Area2D

@export var set_gravity_direction: Vector2
var entered: Array[CollisionEntity]

func _ready():
	body_entered.connect(on_body_entered)
	body_exited.connect(on_body_exited)
	
func on_body_entered(_body: Node):
	if _body is CollisionEntity:
		entered.append(_body)
		
func on_body_exited(_body: Node):
	if _body is CollisionEntity:
		entered.erase(_body)
		
	
func _physics_process(delta):
	for _ce in entered:
		if _ce.is_on_floor():
			var _init_angle = (set_gravity_direction.angle()-PI/2) / (2*PI) * 4
			var _rounded = round(_init_angle)
			print("init_angle: ", _init_angle)
			print("rounded_angle: ", _rounded)
			var _rounded_angle = _rounded / 4 * 2 * PI + PI/2
			var _rounded_direction = Vector2.from_angle(_rounded_angle)
			_ce.set_gravity_direction(_rounded_direction)
			print(_rounded_direction)
