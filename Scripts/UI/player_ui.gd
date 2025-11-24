class_name PlayerUI

extends CanvasLayer

@onready var health_bar = $HPBar/HealthBar
@onready var enemy_health = $EnemyHP/HealthBar
var auta: Auta

var enemy_target: CollisionEntity

static var instance: PlayerUI

func _ready():
	instance = self

func _process(_delta: float) -> void:
	if auta == null: auta = Auta.instance
	var _entstat: EntityStatus = auta.get_node("GenericAttributes/EntityStatus")
	health_bar.value = _entstat.health / _entstat.max_health
	
	if enemy_target != null and is_instance_valid(enemy_target):
		$EnemyTab.visible = true
		$EnemyHP.visible = true
		_entstat = enemy_target.get_node("GenericAttributes/EntityStatus")
		enemy_health.value = _entstat.health / _entstat.max_health
	else:
		$EnemyTab.visible = false
		$EnemyHP.visible = false
