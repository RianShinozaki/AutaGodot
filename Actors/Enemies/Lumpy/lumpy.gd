class_name Lumpy
extends AIEntity

var attack_param: LumpyAttackParameters
var hurt_param: PlayerHurtParameters

signal finished_burrowing

func _ready() -> void:
	super._ready()
	attack_param = parameters["attack"]
	hurt_param = parameters["hurt"]
	
func attack():
	switch_action_state_name("AttackState")
	$Art/AnimationTree.get("parameters/playback").start("Attack", true)
	$SpecialAttributes/HurtManager.power_resistance = hurt_param.power_resistance_tough

func attack_burst():
	var _attack_dir = -1 if $Art.flip_h else 1
	velocity.x = attack_param.attack_speed * _attack_dir
	$SpecialAttributes/HurtManager.power_resistance = hurt_param.power_resistance_compromised

func process_damage(_area):
	$SpecialAttributes/HurtManager.process_hurt(_area)

func do_burrow_animation():
	switch_action_state_name("AttackState")
	$Art/AnimationTree.get("parameters/playback").start("BurrowOut", true)
	await $Art/AnimationTree.animation_finished
	emit_signal("finished_burrowing")
