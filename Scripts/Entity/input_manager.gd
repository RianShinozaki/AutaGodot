class_name InputManager

extends Node2D

@export var read_controller_input: bool
@export var input_direction: Vector2

signal action_a_just_pressed
@export var action_a_pressed: bool
signal action_a_just_released

signal action_b_just_pressed
var action_b_pressed: bool
signal action_b_just_released

signal action_c_just_pressed
var action_c_pressed: bool
signal action_c_just_released

func _process(_delta: float) -> void:
	if read_controller_input:
		input_direction = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
		
		if Input.is_action_just_pressed("A"):
			emit_signal("action_a_just_pressed")
		if Input.is_action_just_released("A"):
			emit_signal("action_a_just_released")
		action_a_pressed = Input.is_action_pressed("A")

		if Input.is_action_just_pressed("B"):
			emit_signal("action_b_just_pressed")
		if Input.is_action_just_released("B"):
			emit_signal("action_b_just_released")
		action_b_pressed = Input.is_action_pressed("B")
		
		if Input.is_action_just_pressed("C"):
			emit_signal("action_c_just_pressed")
		if Input.is_action_just_released("C"):
			emit_signal("action_c_just_released")
		action_c_pressed = Input.is_action_pressed("C")
