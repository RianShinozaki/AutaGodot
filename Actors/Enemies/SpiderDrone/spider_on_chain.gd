extends AIEntity

var cached_sprite_rot: float = 0
var attached_link: ChainLink
var anchor: ChainAnchor
@export var swing_influence: float
var hitbox_data: HitboxData
var aggro: bool = false
var movement_timer: float = 0

func _ready() -> void:
	super._ready()
	apply_physics = false
	hitbox_data = $SpecialAttributes/Hitboxes/Attack.hitbox_data
	
func attach_to_chain(_chainlink: ChainLink):
	anchor = _chainlink.anchor
	attached_link = _chainlink
	anchor.make_links_hot()
	if get_parent() != null:
		get_parent().call_deferred("remove_child", self)
	attached_link.get_node("RigidBody2D").call_deferred("add_child", self)
	position = Vector2.DOWN * 10
	
func move_down_chain(_chainlink: ChainLink):
	attached_link = _chainlink
	if get_parent() != null:
		get_parent().call_deferred("remove_child", self)
	attached_link.get_node("RigidBody2D").call_deferred("add_child", self)
	position = Vector2.DOWN * 10
	
func _physics_process(delta: float) -> void:
	super._physics_process(delta)
	if attached_link == null: return
	
	movement_timer += delta
	if movement_timer >= 0.5:
		movement_timer = 0
		var _next_chain = attached_link.get_lower_chain()
		if _next_chain != attached_link:
			move_down_chain(_next_chain)
	
	if aggro:
		var _link_rb: RigidBody2D = attached_link.get_node("RigidBody2D")
		var hor = 1 if _link_rb.linear_velocity.x > 0 else -1
		if abs(hor) > 0.5:
			var force_dir: Vector2 = attached_link.vector_to_anchor().normalized().rotated(PI/2)
			_link_rb.apply_force(force_dir * sign(hor) * swing_influence)
		
		_link_rb.apply_force(Vector2.DOWN * swing_influence/2)
		hitbox_data.x_knockback = _link_rb.linear_velocity.x
		hitbox_data.y_knockback = _link_rb.linear_velocity.y

func process_damage(_area):
	$SpecialAttributes/HurtManager.process_hurt(_area)
	var _link_rb: RigidBody2D = attached_link.get_node("RigidBody2D")
	var _hitbox_data: HitboxData = _area.hitbox_data
	var _x_knockback: float = _hitbox_data.x_knockback
	if _hitbox_data.x_knockback_is_relative:
		_x_knockback *= -1 if _area.global_position.x > global_position.x else 1
	var _knockback = Vector2(_x_knockback, _hitbox_data.y_knockback/2)
	if _hitbox_data.damage_type == HitboxData.DamageType.HEAVY:
		_link_rb.apply_impulse(_knockback * 5)
	else:
		_link_rb.apply_impulse(_knockback * 5)

func _on_death_manager_just_died():
	anchor.object_attached = null
	anchor.make_links_cool()


func _on_player_detector_body_entered(body):
	aggro = true
	$PlayerDetector/CollisionShape2D.shape.radius = 250
	$Art/AnimationPlayer.play("Aggro")

func _on_player_detector_body_exited(body):
	aggro = false
	$PlayerDetector/CollisionShape2D.shape.radius = 150
	$Art/AnimationPlayer.play("Passive")
	
	
