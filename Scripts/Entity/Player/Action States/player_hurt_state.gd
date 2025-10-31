class_name PlayerHurtState

extends ActionState

var auta: Auta
var inp: InputManager
var anim: AnimationTree

var mov_param: EntityMovementParameters
var jmp_param: EntityJumpParameters
var hurt_param: PlayerHurtParameters

var counter: float = 0

@onready var hit_fx_pool = $"../../SpecialAttributes/ObjectPools/HitFX"

func _ready() -> void:
	super._ready()
	auta = entity as Auta
	inp = entity.get_node("GenericAttributes/InputManager")
	anim = entity.get_node("Art/AnimationTree")
	mov_param = entity.parameters["movement"]
	jmp_param = entity.parameters["jump"]
	hurt_param = entity.parameters["hurt"]
	
func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").set_deferred("shape", mov_param.collision_shape) 
	entity.get_node("EnvironmentBox").position = mov_param.collision_shape_position
	anim.get("parameters/playback").start("Hurt", true)
	entity.get_node("Art/AfterImageGenerator").call("stop_afterimages")
	counter = 0

func initiate_hurt(_hitbox: Hitbox):
	var _hb_data = _hitbox.hitbox_data
	var _x_knockback: float = _hb_data.x_knockback
	if _hb_data.x_knockback_is_relative:
		_x_knockback *= -1 if _hitbox.global_position.x > global_position.x else 1
	entity.velocity.x = _x_knockback
	entity.velocity.y = _hb_data.y_knockback
	var _knockback_total: Vector2 = Vector2(_x_knockback, _hb_data.y_knockback)
	var _len = _knockback_total.length() / 60
	var _dir = _knockback_total.normalized()
	entity.inflict_hitstun(_len, _dir, _hb_data.hitstun_duration)
	GameCamera.instance.shake_screen(_len, 0.25)
	var fx: Node2D = hit_fx_pool.spawn_object()
	var offset: Vector2 = (_hitbox.global_position - entity.global_position).normalized()*4
	if fx != null:
		fx.global_position = entity.global_position + offset
	
	fx = $"../../SpecialAttributes/ObjectPools/ElectricitySpurt".spawn_object()
	if fx != null:
		fx.global_position = entity.global_position + offset
		fx.global_scale.x = sign(_x_knockback)
	
		
	#Draw damage text
	var _dc = $"../../GenericAttributes/DamageCounterParent/DamageCounter"
	var _dca = $"../../GenericAttributes/DamageCounterParent/DamageCounter/AnimationPlayer"
	var _display: float = _hb_data.damage
	if _dca.is_playing():
		_display += _dc.text as float
		_dca.stop()
	_dc.text = str(roundi(_display))
	_dca.play("trigger")
	$"../../GenericAttributes/DamageCounterParent".global_position = global_position + offset + Vector2.UP*2
	
func _physics_process(delta: float) -> void:
	if !active: return
	if !entity.apply_physics: return
	
	entity.accelerate_x(mov_param.get_deceleration(entity) * delta, 0, true)
	entity.gravity = jmp_param.get_gravity(entity, 1.0)
	entity.velocity.y = min(entity.velocity.y, jmp_param.max_falling_speed)
	
	if entity.is_on_wall() and abs(entity.pre_move_velocity.x) > 50:
		var _len = entity.pre_move_velocity.length()/60
		var _dir = entity.pre_move_velocity.normalized()
		
		entity.inflict_hitstun(_len, _dir, 0.25)
		entity.velocity.x = -sign(entity.pre_move_velocity.x) * 30
		entity.velocity.y = 0
		GameCamera.instance.shake_screen(_len/3, 0.25)
	
	if entity.is_on_floor() and (entity.pre_move_velocity.y > 250 or abs(entity.pre_move_velocity).x > 100):
		var _len = entity.pre_move_velocity.length()/60
		var _dir = entity.pre_move_velocity.normalized()
		
		entity.inflict_hitstun(_len, _dir, 0.05)
		entity.velocity.y = -140
		entity.velocity.x = entity.pre_move_velocity.x/2
		GameCamera.instance.shake_screen(_len/3, 0.25)
	
	if abs(entity.velocity.x) < mov_param.get_minimum_speed(entity):
		entity.velocity.x = 0
	
	if entity.velocity.length() < hurt_param.recovery_min_speed:
		counter += delta
		
	if counter >= hurt_param.recovery_time:
		entity.switch_action_state_name("NormalState")
		anim.get("parameters/playback").start("Grounded", true)
		
func process_damage(_area):
	var _hb_data: HitboxData = _area.hitbox_data
	var _hurtstate: PlayerHurtState = entity.switch_action_state_name("HurtState")
	_hurtstate.initiate_hurt(_area as Hitbox)
