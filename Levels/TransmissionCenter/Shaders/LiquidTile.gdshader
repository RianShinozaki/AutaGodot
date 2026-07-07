shader_type canvas_item;

uniform sampler2D noise_texture: filter_nearest, repeat_enable;
uniform sampler2D screen_texture: hint_screen_texture;
uniform float distortion = 0.1;
uniform float speed = 0.2;
uniform float offset;
varying vec2 world_coords;

uniform vec2 divisor;

void vertex() {
	// Called for every vertex the material is visible on.
	world_coords = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy;
	VERTEX.y += sin(TIME*2.0 + world_coords.x*1024.0)*2.0;
	VERTEX.y += 5.0;
}

void fragment() {
	vec2 uv =  vec2(speed,speed/2.0) * TIME;
	vec2 uv2 =  vec2(-speed,speed/2.0) * TIME;
	float x = (floor(world_coords.x)/128.0);
	float y = (floor(world_coords.y)/128.0);
	
	vec2 noise = 2.0 * texture(noise_texture, uv + vec2(x, y)).rg - vec2(1.0);
	vec2 noise2 = 2.0 * texture(noise_texture, uv + vec2(x, y)).rg - vec2(1.0);
	COLOR = texture(TEXTURE, UV + noise);
	COLOR += texture(screen_texture, SCREEN_UV + noise * distortion);
	COLOR += vec4(noise2.x, 0.0 , noise2.y , 0.0);
	//COLOR = texture(TEXTURE, UV + noise*distortion);
	// Called for every pixel the material is visible on.
}

//void light() {
	//LIGHT = vec4(1.0, 0.0, 0.0, 1.0);
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
