class_name VoidStar extends CharacterBody2D

@export_range(0, 1) var bounce_coef: float = 1.0
@export var initial_velocity: float = 150
@export var lifetime = 5
@export var gravity: float = 400
@export var attract_force: float = 2500

var counter: float = 0
var auta: Auta
var attract: bool = false

func _ready():
	await get_tree().process_frame
	auta = Auta.instance
	
func _physics_process(delta):
	var _collision = move_and_collide(velocity * delta)
	
	if not attract:
		velocity += Vector2.DOWN * gravity * delta
		velocity -= velocity * 3 * delta
		if _collision:
			velocity = velocity.bounce(_collision.get_normal()) * bounce_coef
			
	if attract:
		velocity -= velocity * 6 * delta
		var _dir = (auta.global_position - global_position).normalized()
		var _mag = (auta.global_position - global_position).length()
		velocity += _dir * attract_force * delta
		if _mag < 8:
			queue_free()


func _on_orb_detector_area_entered(area):
	attract = true
