class_name PlayerUI

extends CanvasLayer

@onready var health_bar = $AutaUI/HealthBar
var auta: Auta

func _process(_delta: float) -> void:
	if auta == null: auta = Auta.instance
	var _entstat: EntityStatus = auta.get_node("GenericAttributes/EntityStatus")
	health_bar.value = _entstat.health / _entstat.max_health
	
