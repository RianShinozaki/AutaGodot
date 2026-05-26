class_name PlayAnimationEvent extends CutsceneEvent

@export var anim_name: StringName
	
func trigger(_caller: CutsceneSequence):
	var _anim_player: AnimationPlayer = _caller.get_node("AnimationPlayer")
	_anim_player.play(anim_name)
	await _anim_player.animation_finished
	emit_signal("finished")
