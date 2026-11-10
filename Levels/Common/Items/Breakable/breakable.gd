class_name Breakable 
extends Area2D

var clash_sound: AudioStream = preload("res://Levels/Common/Items/Breakable/MetalClash.wav")
@export var break_sound: AudioStream
@export var obj_particle: PackedScene
@export var particles: int = 5
@export var destroy: bool = true
@export var tough: bool = false
var broken = false

signal just_broken

@export var should_signal_strike: bool = false
signal struck

func _ready():
	area_entered.connect(on_area_entered)
	
func on_area_entered(_area: Area2D):
	if broken: return
	if not _area is Hitbox: return
	var _hb: HitboxData = _area.hitbox_data
	
	if tough and _hb.damage_type != HitboxData.DamageType.HEAVY: 
		if should_signal_strike: emit_signal("struck")
		SFXController.play_sound(clash_sound, global_position)
		return
	
	if break_sound:
		SFXController.play_sound(break_sound, global_position)
		
	if obj_particle:
		for i in range(particles):
			var _particle = obj_particle.instantiate()
			get_parent().call_deferred("add_child", _particle)
			_particle.set_deferred("global_position", global_position + $Art.get_rect().get_center() )
			_particle.velocity = Vector2.from_angle(randf_range(0, 2*PI))*_particle.initial_velocity
			var _x_knockback = _hb.x_knockback
			if _hb.x_knockback_is_relative:
				_x_knockback *= -1 if _area.global_position.x > global_position.x else 1
			
			_particle.velocity += Vector2(_x_knockback, _hb.y_knockback).normalized()*_particle.initial_velocity*0.55
	
	broken = true
	emit_signal("just_broken")
	if destroy:
		queue_free()
	else:
		$Art.frame = 1
		if get_node_or_null("Art/PointLight2D"):
			$Art/PointLight2D.visible = false
		if get_node_or_null("EnvCollision"):
			$EnvCollision/CollisionShape2D.set_deferred("disabled", true)
