extends Control

@export var play_scene : PackedScene
@export var options_scene : PackedScene
@export var credits_scene : PackedScene

func _on_btn_play_pressed() -> void:
	get_tree().change_scene_to_packed(play_scene)

func _on_btn_options_pressed() -> void:
	get_tree().change_scene_to_packed(options_scene)

func _on_btn_credits_pressed() -> void:
	get_tree().change_scene_to_packed(credits_scene)


func _on_btn_quit_pressed() -> void:
	get_tree().quit()
