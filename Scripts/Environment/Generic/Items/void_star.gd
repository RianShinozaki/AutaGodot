class_name VoidStar extends Area2D

@export var dampen: float
@export var attract_force: float
var auta: Auta
var attract: bool = false
var velocity: Vector2

func _ready():
	await get_tree().process_frame
	auta = Auta.instance

func launch(_angle: float):
	var _dir = Vector2.from_angle(_angle)
	velocity = _dir * 150
	
func _physics_process(delta):
	global_position += velocity * delta
	velocity -= velocity.normalized() * dampen * delta
	
	if auta == null: return
	if attract:
		var _dir = (auta.global_position-global_position).normalized()
		velocity += _dir * attract_force * delta
	
	if (auta.global_position-global_position).length() < 24:
		attract = true
		
	if (auta.global_position-global_position).length() < 8:
		call_deferred("queue_free")

func _on_orb_detector_area_entered(area):
	attract = true
