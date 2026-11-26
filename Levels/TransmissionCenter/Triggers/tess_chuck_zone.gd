extends Area2D

@export var active: bool = false
var entered: bool = false
var chuck_timer: float = 0
@export var chuck_timer_max: float
var lamp_object = preload("res://Levels/TransmissionCenter/Obstacles/thrown_lamp.tscn")

func _ready():
	body_entered.connect(on_body_entered)
	body_exited.connect(on_body_exited)

func _process(delta):
	if entered and active:
		chuck_timer += delta
		if chuck_timer >= chuck_timer_max:
			chuck_lamp()
			chuck_timer = 0
	
func on_body_entered(_body: Node2D):
	entered = true

func on_body_exited(_body: Node2D):
	entered = false

func chuck_lamp():
	print("throwing")
	var _auta_pos: Vector2 = Auta.instance.global_position
	var _throw_pos_y = _auta_pos.y - 400
	var _throw_pos_x = _auta_pos.x + randf_range(-100, 100)
	var _lamp = lamp_object.instantiate()
	add_child(_lamp)
	_lamp.global_position = Vector2(_throw_pos_x, _throw_pos_y)
	_lamp.initiate()
