extends Area2D

func _on_area_entered(area):
	if Auta.instance.current_action_state.name == "ConstellationState":
		Auta.instance.switch_action_state_name("NormalState")
