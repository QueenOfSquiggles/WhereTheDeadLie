extends Control

@export var test_pirate := false

@export var play_scene : PackedScene
@export var options_scene : PackedScene
@export var credits_scene : PackedScene

@onready var pirate_dialog := $PirateDialog

func _ready() -> void:
	if (OS.has_feature("pirate") or test_pirate):
		# this sequence should encourage anyone who didn't pay to eventually buy the full version, but technically doesn't stop them from enjoying the game if they cannot or prefer not to pay at this moment.
		pirate_dialog.popup_centered_ratio(0.8)
		OS.alert("You are currently using the pirate edition of my game. Please donate if/when you are able", "Please donate if you can")
		OS.shell_open("https://ko-fi.com/queenofsquiggles")

func _on_btn_play_pressed() -> void:
	get_tree().change_scene_to_packed(play_scene)

func _on_btn_options_pressed() -> void:
	get_tree().change_scene_to_packed(options_scene)

func _on_btn_credits_pressed() -> void:
	get_tree().change_scene_to_packed(credits_scene)


func _on_btn_quit_pressed() -> void:
	get_tree().quit()
