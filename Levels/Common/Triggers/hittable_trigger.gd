extends Hurtbox

@onready var active: bool = false
@onready var art := $Sprite2D

signal triggered(active: bool)

func _on_area_entered(area):
	if area is Hitbox:
		var _hb: Hitbox = area as Hitbox
		var _hb_data = _hb.hitbox_data
		var _dist_to_cam = (GameCamera.instance.global_position - global_position).length()
			#Process X knockback direction
		var _x_knockback: float = _hb_data.x_knockback
		if _hb_data.x_knockback_is_relative:
			_x_knockback *= -1 if _hb.global_position.x > global_position.x else 1
		
		#Get knockback attributes for hitstun
		var _knockback_total: Vector2 = Vector2(_x_knockback, _hb_data.y_knockback)
		var _len = _knockback_total.length() / 60
		var _dir = _knockback_total.normalized()
		
		_hb.on_impact(self)
		active = not active
		art.frame = 1 - art.frame
		emit_signal("triggered", active)
		
		if _dist_to_cam < 400:
			GameCamera.instance.shake_screen(_len, 0.25)
			if _hb_data.impact_sound:
				SFXController.play_sound(_hb_data.impact_sound, global_position, 1, randf_range(0.9, 1.1))
