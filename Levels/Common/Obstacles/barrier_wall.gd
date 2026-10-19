class_name BarrierWall extends AnimatableBody2D

@export var active: bool = false

@export var off_position: Vector2
@export var on_position: Vector2

@export var move_speed: float = 100

var target_position: Vector2

func _ready():
	target_position = on_position if active else off_position
	position = target_position

func _physics_process(_delta):
	position = position.move_toward(target_position, _delta * move_speed)

func on_triggered(_active: bool):
	active = _active
	target_position = on_position if active else off_position
