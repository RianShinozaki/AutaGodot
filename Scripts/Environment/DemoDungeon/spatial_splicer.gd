extends Area2D

@export var splice_offset_tiles: Vector2
@export var zoom_out: bool
var splice_offset_tiles_orig: Vector2
var splice_active = false
var target: Node2D
@onready var sprite_dupe = $"../SpriteDupe"

signal splice_activate
signal splice_deactivate

var line_tween: PropertyTweener = null
var line_thickness: float = 0

var line_len: float
var collider_list: Array[Node2D]

func _ready() -> void:
	splice_offset_tiles_orig = splice_offset_tiles
	body_entered.connect(on_body_entered)
	$Portal.visible = true
	$"../SpriteDupe/Area2D".body_entered.connect(on_sprite_body_entered)
	$"../SpriteDupe/Area2D".body_exited.connect(on_sprite_body_exited)
	
func _physics_process(_delta: float) -> void:
	if splice_active:
		sprite_dupe.global_position = target.global_position + splice_offset_tiles*16
		queue_redraw()
	
func on_body_entered(body: Node2D) -> void:
	if body is Auta and not splice_active:
		splice_offset_tiles = splice_offset_tiles_orig
		target = body
		splice_active = true
		sprite_dupe.active = true
		sprite_dupe.visible = true
		sprite_dupe.target = body.get_node("Art")
		emit_signal("splice_activate")
		$Portal.visible = false
		body.orbed.connect(on_orbed)
		on_orbed()
		GameCamera.instance.targets.append(sprite_dupe)
		if zoom_out: GameCamera.instance.lerp_zoom(2)

func on_sprite_body_entered(body: Node2D):
	collider_list.append(body)

func on_sprite_body_exited(body: Node2D):
	collider_list.erase(body)
	
func deactivate() -> void:
	emit_signal("splice_deactivate")
	splice_active = false
	sprite_dupe.active = false
	sprite_dupe.visible = false
	target.orbed.disconnect(on_orbed)
	$Portal.visible = true
	GameCamera.instance.targets.erase(sprite_dupe)
	if zoom_out: GameCamera.instance.lerp_zoom(3)
	
func on_orbed() -> void:
	if collider_list.is_empty():
		target.global_position += splice_offset_tiles*16
		splice_offset_tiles = -splice_offset_tiles
	else:
		target.velocity = Vector2.ZERO
	line_thickness = 12
	line_len = 1
	line_tween = create_tween().tween_property(self, "line_thickness", 0, 0.3).set_trans(Tween.TRANS_BOUNCE)
	create_tween().tween_property(self, "line_len", 0, 0.3).set_trans(Tween.TRANS_CUBIC)
	$"../SpriteDupe/ScreenshakeEmitter".EmitShake()
	
func _draw() -> void:
	if splice_active:
		draw_set_transform_matrix(global_transform.affine_inverse())
		draw_line(target.global_position, target.global_position + splice_offset_tiles*16 * line_len, Color.DEEP_PINK, line_thickness)
