extends AIEntity

var attack_param: LumpyAttackParameters
var hurt_param: PlayerHurtParameters

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
