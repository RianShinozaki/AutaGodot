extends AIEntity

func process_damage(_area):
	$SpecialAttributes/HurtManager.process_hurt(_area)
