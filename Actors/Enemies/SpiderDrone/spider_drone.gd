extends AIEntity

func process_damage(_area):
	$SpecialAttributes/HurtManager.process_hurt(_area)

func _ready() -> void:
	up_direction = Vector2.from_angle(rotation - PI/2)
	gravity_direction = -up_direction
