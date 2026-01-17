shader_type spatial;

uniform sampler2D albedo_texture : source_color, repeat_enable;
uniform vec2 Tiling = vec2(1.0, 1.0);
uniform sampler2D noise_texture;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Tiling
	vec2 tiled_uv = UV * Tiling;
	
	// Sample the texture at two different scales
	vec3 tex1 = texture(albedo_texture, tiled_uv * 1.0).rgb;
	vec3 tex2 = texture(albedo_texture, tiled_uv * 0.731 + vec2(0.5, 0.2)).rgb;
	
	// Mix them together using a large-scale noise texture or a mask
	float mask = texture(noise_texture, UV * 0.1).r;
	ALBEDO = mix(tex1, tex2, mask);
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
