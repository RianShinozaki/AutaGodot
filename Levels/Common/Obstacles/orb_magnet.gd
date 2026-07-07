extends Area2D

var entered: bool = false
@export var pull_force: float
@export var speed_cap: float
@export var pull_curve: Curve
var auta: Auta

func _ready():
	await get_tree().process_frame
	auta = Auta.instance

func _process(delta):
	if entered:
		var _length = (global_position - auta.global_position).length()
		if _length == 0: 
			_length = 0.1
		
		var _dir = (global_position - auta.global_position).normalized()
		auta.velocity += _dir * pull_force * delta * pull_curve.sample(_length/72)
		#auta.velocity += Vector2(_dir.y, _dir.x) * pull_force * delta * pull_curve.sample(_length/72)
		auta.velocity.y -= auta.gravity*delta
		auta.get_action_state_name("OrbState").can_control_exit = false
		if auta.velocity.length() > speed_cap:
			print(auta.velocity.length())
			print(speed_cap)
			auta.velocity = speed_cap * auta.velocity.normalized()


func _on_area_entered(_area):
	entered = true


func _on_area_exited(_area):
	entered = false
