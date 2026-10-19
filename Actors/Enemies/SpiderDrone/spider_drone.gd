extends AIEntity

var cached_sprite_rot: float = 0

func process_damage(_area):
	$SpecialAttributes/HurtManager.process_hurt(_area)

func _ready() -> void:
	cached_sprite_rot = global_rotation
	up_direction = Vector2.from_angle(rotation - PI/2)
	gravity_direction = -up_direction

func _physics_process(delta: float) -> void:
	super._physics_process(delta)
	cached_sprite_rot = rotate_toward(cached_sprite_rot, global_rotation, delta*20)
	get_node("Art").global_rotation = cached_sprite_rot
