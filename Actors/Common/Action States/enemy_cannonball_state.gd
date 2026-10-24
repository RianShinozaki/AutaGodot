extends ActionState

@onready var slam_fx_pool = $"../../SpecialAttributes/ObjectPools/WallSlam"
@onready var boom_fx_pool = $"../../SpecialAttributes/ObjectPools/SonicBoom"
var inp: InputManager
var anim: AnimationTree
var grabbed: bool = false
var parent_cache: Node
var cannon_param: CannonballParameters
var life_counter: float = 0
@onready var death_manager = $"../../SpecialAttributes/DeathManager"
@export var cannonball_crash: AudioStream

var sonic_boom_delay: float = 0.2
var sonic_boom_counter: float = 0
func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	anim = entity.get_node("Art/AnimationTree")
	entity.just_bounced.connect(just_bounced)
	entity.impact.connect(on_impact)
	cannon_param = entity.parameters["cannonball"]
	
func _start() -> void:
	super._start()
	entity.collision_mode = entity.COLLISION_MODE_BOUNCE
	anim.get("parameters/playback").start("Thrown", true)
	entity.gravity = 0
	$"../../Art/AfterImageGenerator".start_afterimages()
	
func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	life_counter += delta
	sonic_boom_counter += delta
	if sonic_boom_counter >= sonic_boom_delay:
		sonic_boom_counter -= sonic_boom_delay
		var _boom = boom_fx_pool.spawn_object()
		if is_instance_valid(_boom):
			_boom.global_position = entity.global_position
			_boom.global_rotation = entity.velocity.angle() + PI/2
		
	var _art: Sprite2D = entity.get_node("Art")
	_art.rotate(25 * sign(entity.velocity.x) * delta)
	if life_counter < cannon_param.gravity_delay:
		entity.gravity = 0
	elif life_counter >= cannon_param.gravity_delay:
		entity.gravity = cannon_param.gravity
	if life_counter >= cannon_param.lifetime:
		death_manager.death()

func _end() -> void:
	super._end()

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)

func just_bounced(normal: Vector2, velocity: Vector2, _position: Vector2):
	if not active: return
	
	SFXController.play_sound(cannonball_crash, global_position)
	var _slam = slam_fx_pool.spawn_object()
	GameCamera.instance.shake_screen(2, 0.25)
	if _slam != null:
		_slam.global_position = _position
		_slam.global_rotation = normal.angle() + PI/2
	
func on_impact(_hitbox: Hitbox, _hurtbox: Hurtbox):
	if not active: return
	
	var _dir = (_hitbox.global_position - entity.global_position).normalized()
	entity.velocity = entity.velocity.bounce(_dir)
