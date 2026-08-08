extends ActionState

@onready var land_fx_pool = $"../../SpecialAttributes/ObjectPools/Land"
var inp: InputManager
var anim: AnimationTree

var grabbed: bool = false

@export var auto_flip: bool = true
var parent_cache: Node

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	anim = entity.get_node("Art/AnimationTree")
	entity.just_grounded.connect(just_grounded)
	
func _start() -> void:
	super._start()
	anim.get("parameters/playback").start("Thrown", true)
	entity.gravity = 0
	
func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	var _art: Sprite2D = entity.get_node("Art")
	_art.rotate(25 * sign(entity.velocity.x) * delta)

func _end() -> void:
	super._end()
	
#Grounded animation handler
func just_grounded(_normal: Vector2, _velocity: Vector2):
	var fx: Node2D = land_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.get_last_slide_collision().get_position()
		fx.rotation = entity.get_floor_angle() * sign(entity.get_floor_normal().x)

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)
