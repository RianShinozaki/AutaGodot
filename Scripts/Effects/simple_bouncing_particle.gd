extends CharacterBody2D

@export_range(0, 1) var bounce_coef: float = 1.0
@export var initial_velocity: float = 150
@export var lifetime = 5
@export var gravity: float = 400
@export var attract_force: float = 2500
@export var absorb_heal_amount: float = 0.5

var attract = false
var auta: Auta
var counter: float = 0

func _ready():
	auta = Auta.instance

func _physics_process(delta):
	var _collision = move_and_collide(velocity * delta)
	
	if not attract:
		velocity += Vector2.DOWN * gravity * delta
		
		if _collision:
			velocity = velocity.bounce(_collision.get_normal()) * bounce_coef
		counter += delta
		if counter >= lifetime:
			call_deferred("queue_free")
			
	if attract:
		velocity -= velocity * 6 * delta
		var _dir = (auta.global_position - global_position).normalized()
		var _mag = (auta.global_position - global_position).length()
		velocity += _dir * attract_force * delta
		if _mag < 8:
			auta.get_node("GenericAttributes/EntityStatus").add_health(absorb_heal_amount)
			queue_free()


func _on_orb_detector_area_entered(area):
	attract = true
