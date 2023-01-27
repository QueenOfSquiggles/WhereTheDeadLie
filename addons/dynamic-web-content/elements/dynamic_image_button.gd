extends PanelContainer
class_name DynamicImageButton
#	Dynamic Image Button
#	- - - - -
#	An image button that acquires a data block from the web to load an image and a link to open upon being clicked.
#	Optionally a subtitle can be added

# supplementary signal to allow for extra features such as SFX and the like
signal on_pressed

@export var element_id := "default-element"
@export var placeholder_image : Texture

# content
var button : TextureButton
var link : String
var subtitle : Label

func _ready() -> void:
	await(RenderingServer.frame_post_draw)

	# setup structure
	var vbox := VBoxContainer.new()
	add_child(vbox)
	button = TextureButton.new()
	button.size_flags_vertical = Control.SIZE_EXPAND_FILL
	button.stretch_mode = TextureButton.STRETCH_SCALE
	button.ignore_texture_size = true
	vbox.add_child(button)
	subtitle = Label.new()
	subtitle.text = "unloaded"
	subtitle.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART
	vbox.add_child(subtitle)
	
	if placeholder_image:
		_set_button_texture(placeholder_image)
	else:
		var default_icon := load("res://icon.svg")
		if default_icon:
			_set_button_texture(default_icon)
	
	var lambda_btn := func():
		OS.shell_open(link)
	button.pressed.connect(lambda_btn)
	
	# Link with manager
	DynamicContentManager.request_content_refresh.connect(_refresh_content)
	if DynamicContentManager.cache_enabled:
		_load_cached()
	if DynamicContentManager.auto_refresh_on_ready:
		_refresh_content()
	

func _refresh_content() -> void:
	var http := HTTPRequest.new()
	add_child(http)
	
	http.request_completed.connect(_http_request_data)
	var lambda_kill = func(result: int, response_code: int, headers: PackedStringArray, body: PackedByteArray):
		http.queue_free()
	http.request_completed.connect(lambda_kill, CONNECT_DEFERRED)
	
	var request_path := _get_web_path()
	if request_path == "":
		return
	var err = http.request(request_path)
	if err != OK:
		push_warning("Failed to make a simple HTTP Request to path '%s'. \
			Please ensure the application is allowed to make requests" % request_path)
	


func _get_web_path() -> String:
	var request_path := ProjectSettings.get_setting("DynamicContentLoading/links/webpage_root", null) as String
	if request_path == null:
		push_warning("webpage_root setting is currently not set! \
			Turn on advanced options in the Project Settings to assign the webpage root!")
		return ""
	if not request_path.ends_with("/"):
		request_path += "/"
	request_path += ProjectSettings.get_setting("DynamicContentLoading/links/game_id", "test") as String
	request_path += ".json"
	return request_path

func _http_request_data(result: int, response_code: int, headers: PackedStringArray, body: PackedByteArray) -> void:
	if result != OK:
		push_warning("Dynamic Content Element '%s' failed to load http data, \
			error code %s" % [name, str(response_code)])
		return
	var json_text := body.get_string_from_utf8()
	var json_data : Dictionary = JSON.parse_string(json_text)
	if json_data.has(element_id):
		_load_from_data(json_data[element_id])
	else:
		push_error("Element ID '%s' was not found in remote data file" % element_id)

func _load_from_data(data : Dictionary) -> void:
	link = _get_safe(data, "link", "https://youtu.be/2DFmUk1-Q7k") # we have fun here :)
	button.tooltip_text = link
	var img := _get_safe(data, "img", "null") as String

	if img != "null":
		img.replace("${HOME}", ProjectSettings.get_setting("DynamicContentLoading/links/webpage_root", null) as String)
		_load_web_texture(img)

	var txt := _get_safe(data, "text", "")
	if txt != "":
		subtitle.text = txt
	if DynamicContentManager.cache_enabled:
		_save_to_cache(data)

func _save_to_cache(data : Dictionary) -> void:
	var dir := DirAccess.open(DynamicContentManager.CACHE_PATH)
	dir.make_dir_recursive(element_id)
	
	# save localized data json
	var file := FileAccess.open(DynamicContentManager.CACHE_PATH + element_id + "/data.json", FileAccess.WRITE)
	var n_data := data.duplicate()
	if n_data.has("img") and button.texture_normal != null:
		n_data.img = DynamicContentManager.CACHE_PATH + element_id + "/img." + n_data.img.get_extension()
		# save out runtime loaded texture
		var err := ResourceSaver.save(button.texture_normal, n_data.img)
		if err != OK:
			push_warning("Failed to save loaded texture to cache folder")
	file.store_string(JSON.stringify(n_data, "\t"))

func _get_safe(data : Dictionary, var_name : String, default_val : Variant) -> Variant:
	if data.has(var_name):
		return data[var_name]
	return default_val

func _load_web_texture(path : String) -> void:
	var http := HTTPRequest.new()
	add_child(http)
	http.download_file = DynamicContentManager.CACHE_PATH + element_id + "/img."  + path.get_extension()
	
	var err = http.request(path)
	if err != OK:
		push_error("Failed to make HTTP request")
		return
	await(http.request_completed)
	var image = Image.load_from_file(DynamicContentManager.CACHE_PATH + element_id + "/img." + path.get_extension()) # allows loading from user folder
	if image:
		var texture = ImageTexture.new()
		texture.set_image(image)
		_set_button_texture(texture)

#	var lambda_load_buffer := func(result: int, response_code: int,\
#			headers: PackedStringArray, body: PackedByteArray):
#		var texture = _load_texture_resource(path, body)
#		if texture:
#			_set_button_texture(texture)
#		http.queue_free()

func _load_texture_resource(path : String, buffer : PackedByteArray) -> ImageTexture:
	var ext := path.get_extension().to_lower()
	var image := Image.new()
	var err := OK
	match ext:
		"jpg":
			err = image.load_jpg_from_buffer(buffer)
		"jpeg":
			err = image.load_jpg_from_buffer(buffer)
		"png":
			err = image.load_png_from_buffer(buffer)
		"webp":
			err = image.load_webp_from_buffer(buffer)
		"bmp":
			err = image.load_bmp_from_buffer(buffer)
		"tga":
			err = image.load_tga_from_buffer(buffer)
	if err != OK:
		push_warning("Problem processing image buffer data. Possible corrupted file??")
		return null
	var texture := ImageTexture.new()
	texture.create_from_image(image)
	return texture


func _load_cached() -> void:
	if not DynamicContentManager.cache_enabled:
		return
	var dir := DirAccess.open(DynamicContentManager.CACHE_PATH)
	if not dir.dir_exists(element_id):
		push_warning("No cache data exists. This should happen the first time content is loaded")
		return
	if not dir.file_exists(element_id + "/data.json"):
		return
	var file := FileAccess.open(DynamicContentManager.CACHE_PATH + element_id + "/data.json", FileAccess.READ)
	var json_data :Dictionary = JSON.parse_string(file.get_as_text())
	_load_from_cached_data(json_data)

func _load_from_cached_data(data : Dictionary) -> void:
	link = _get_safe(data, "link", "https://youtu.be/2DFmUk1-Q7k") # we have fun here :)
	button.tooltip_text = link

	var img := _get_safe(data, "img", "null") as String

	if img != "null" and FileAccess.file_exists(img):
		var image = Image.load_from_file(img) # allows loading from user folder
		if image:
			var texture = ImageTexture.new()
			texture.set_image(image)
			_set_button_texture(texture)
		else:
			push_warning("Failed to load cached image file at %s" % img)

	var txt := _get_safe(data, "text", "")
	if txt != "":
		subtitle.text = txt
	

func _set_button_texture(tex : Texture) -> void:
	# assign as a static button. No feedback
	# maybe we could generate a darkened copy of the textures at runtime? That would be cool
	# not too difficult actually. Just feeling lazy rn
	button.texture_disabled = tex
	button.texture_focused = tex
	button.texture_hover = tex
	button.texture_normal = tex
	button.texture_pressed = tex
