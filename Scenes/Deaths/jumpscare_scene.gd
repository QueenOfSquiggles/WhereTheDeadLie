extends Node3D

@export_file("*.tscn") var main_menu_scene := ""

func _return_to_main_menu():
	get_tree().change_scene_to_file(main_menu_scene)
