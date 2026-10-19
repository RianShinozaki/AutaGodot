extends AnimationPlayer

@export var anim_name: StringName
@export var one_shot: bool = true
var triggered: bool = false

func on_triggered():
	if triggered: return
	play(anim_name)
	if one_shot: triggered = true
