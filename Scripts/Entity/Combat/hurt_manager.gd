class_name HurtManager

extends Node2D

@export var auto_hook: bool = true

## Basically the super-armor stat
@export var power_resistance: int
@export var capture_ui: bool = true

@onready var object_pools = $"../ObjectPools"

var entity: CollisionEntity

signal staggered

func _ready() -> void:
	if auto_hook: entity = get_parent().get_parent()
	
func process_hurt(_hitbox: Area2D):
	#Get hitbox data
	var _hb_data: HitboxData = _hitbox.hitbox_data
	var _is_player: bool = entity is Auta
	
	#Process X knockback direction
	var _x_knockback: float = _hb_data.x_knockback
	if _hb_data.x_knockback_is_relative:
		_x_knockback *= -1 if _hitbox.global_position.x > global_position.x else 1
	
	#Apply all knockback
	if power_resistance == _hb_data.knockback_power:
		entity.velocity.x += _x_knockback
		entity.velocity.y += _hb_data.y_knockback
	elif power_resistance < _hb_data.knockback_power:
		if sign(entity.velocity.x) != sign(_x_knockback):
			entity.velocity.x = _x_knockback
		else:
			entity.velocity.x += _x_knockback
		
		if sign(entity.velocity.y) != sign(_hb_data.y_knockback):
			entity.velocity.y = _hb_data.y_knockback
		else:
			entity.velocity.y += _hb_data.y_knockback
			
		entity.switch_action_state_name("HurtState")
		emit_signal("staggered")
	
	#Get knockback attributes for hitstun
	var _knockback_total: Vector2 = Vector2(_x_knockback, _hb_data.y_knockback)
	var _len = _knockback_total.length() / 60
	var _dir = _knockback_total.normalized()
	entity.inflict_hitstun(_len, _dir, _hb_data.hitstun_duration)
	GameCamera.instance.shake_screen(_len, 0.25)
	
	var offset: Vector2 = (_hitbox.global_position - entity.global_position).normalized()*4
	
	#Create FX
	if _hb_data.impact_fx != "":
		var pool = object_pools.get_node(_hb_data.impact_fx)
		var fx: Node2D = pool.spawn_object()
		if fx != null:
			fx.global_position = entity.global_position + offset
			fx.global_scale.x = sign(_x_knockback)
	
	if power_resistance < _hb_data.knockback_power:
		var fx = object_pools.get_node("BloodSpurt").spawn_object()
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
	
	#Capture UI
	if capture_ui:
		PlayerUI.instance.captured_enemy(entity)
	
	#Respawn if player
	if _is_player and _hb_data.force_respawn:
		entity.quick_respawn()
