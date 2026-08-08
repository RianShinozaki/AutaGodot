class_name GameWorld extends CanvasModulate

@export var level: String
@export var do_cutscenes: bool = true

@export var bgm: AudioStream
@export var bgm_volume: float
static var instance: GameWorld

func _enter_tree():
	print("loading data...")
	SaveSystem.LevelChangeString(level)
	SaveSystem.LoadData(2, 0)
	if bgm:
		MusicController.start_music(bgm, bgm_volume)
	instance = self
	print("data load success!")

func save():
	Global.auta_hp = Auta.instance.get_node("GenericAttributes/EntityStatus").health
	SaveSystem.SaveData(2, 0)
	Global.save()
	print("SAVING GAME")
