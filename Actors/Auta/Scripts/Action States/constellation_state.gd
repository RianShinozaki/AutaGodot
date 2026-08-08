extends ActionState

var auta: Auta
var inp: InputManager
var anim: AnimationTree
var orb_param: AutaOrbParameters
var const_param: ConstellationParameters

var touching_star: bool = false

@onready var orb_reform_fx_pool = $"../../SpecialAttributes/ObjectPools/OrbReform"
@onready var aim_arrow = $AimPivot

@onready var orb_area_shape = $"../OrbState/OrbGrab/CollisionShape2D"
func _ready():
	super._ready()
	auta = entity as Auta
	inp = entity.get_node("GenericAttributes/InputManager")
	anim = entity.get_node("Art/AnimationTree")
	orb_param = entity.parameters["orb"]
	const_param = entity.parameters["constellation"]
	aim_arrow.visible = false
	inp.action_a_just_pressed.connect(on_a_pressed)
	inp.action_b_just_pressed.connect(on_b_pressed)
	
	

func _start() -> void:
	super._start()
	entity.collision_mode = CollisionEntity.COLLISION_MODE_BOUNCE
	entity.gravity = 0
	entity.velocity = Vector2.ZERO
	anim.get("parameters/playback").start("Orb", true)
	entity.get_node("Art").rotation = 0
	entity.get_node("EnvironmentBox").set_deferred("shape", orb_param.collision_shape)
	entity.get_node("EnvironmentBox").position = orb_param.collision_shape_position
	entity.get_node("Art/AfterImageGenerator").call("start_afterimages")
	aim_arrow.visible = true
	#aim_arrow.rotation = entity.pre_move_velocity.angle()
	touching_star = true
	orb_area_shape.set_deferred("disabled", false)
	GameCamera.instance.lerp_zoom(3, 1.5)
	var fx: Node2D = orb_reform_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.global_position;

func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	
	var _hor = inp.input_direction.x
	aim_arrow.rotate(_hor * delta * const_param.rotate_speed)
	
	entity.velocity = entity.velocity.move_toward(Vector2.ZERO, const_param.speed_dampen*delta)
	#if entity.velocity.length() < 100 and not touching_star:
	#	entity.switch_action_state_name("NormalState")
	
func on_a_pressed():
	if not active: return
	if not touching_star: return
	
	entity.velocity = const_param.launch_speed * Vector2.from_angle(aim_arrow.rotation)
	touching_star = false
	aim_arrow.visible = false

func on_b_pressed():
	if not active: return
	
	entity.switch_action_state_name("NormalState")
	
func _end() -> void:
	super._end()
	entity.collision_mode = CollisionEntity.COLLISION_MODE_FLOOR
	anim.get("parameters/playback").start("Jump", true)
	auta.can_orb = false
	auta.recharge_orb = false
	auta.orb_timer = orb_param.orb_recharge_time
	
	aim_arrow.visible = false
	orb_area_shape.set_deferred("disabled", true)
	GameCamera.instance.lerp_zoom(4, 1.5)
	GameCamera.instance.retarget(self)
	var fx: Node2D = orb_reform_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.global_position;
