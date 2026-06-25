shader_type canvas_item;

uniform sampler2D viewport_texture : repeat_disable, filter_linear;
uniform sampler2D base_texture : repeat_disable, filter_linear;

void fragment() {
    vec2 uv = SCREEN_UV;
    vec4 reflection = texture(viewport_texture, uv);
    reflection.a *= 0.4;
	vec4 base = texture(TEXTURE, UV);

    COLOR = base;
	COLOR.rgb = mix(base.rgb, reflection.rgb, reflection.a);
	COLOR.a = base.a;

}