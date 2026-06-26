class_name Breakable 
extends Area2D
@export var break_sound: AudioStream
@export var obj_particle: PackedScene
@export var particles: int = 5
@export var destroy: bool = true
var broken = false

func _ready():
	area_entered.connect(on_area_entered)
	
func on_area_entered(_area: Area2D):
	if broken: return
	if not _area is Hitbox: return
	
	if break_sound:
		SFXController.play_sound(break_sound, global_position)
		
	if obj_particle:
		for i in range(particles):
			var _hb: HitboxData = _area.hitbox_data
			var _particle = obj_particle.instantiate()
			get_parent().call_deferred("add_child", _particle)
			_particle.set_deferred("global_position", global_position + $Art.get_rect().get_center() )
			_particle.velocity = Vector2.from_angle(randf_range(0, 2*PI))*_particle.initial_velocity
			var _x_knockback = _hb.x_knockback
			if _hb.x_knockback_is_relative:
				_x_knockback *= -1 if _area.global_position.x > global_position.x else 1
			
			_particle.velocity += Vector2(_x_knockback, _hb.y_knockback).normalized()*_particle.initial_velocity*0.55
	
	broken = true
	if destroy:
		queue_free()
	else:
		$Art.frame = 1
