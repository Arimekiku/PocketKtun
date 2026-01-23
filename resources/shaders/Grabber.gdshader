shader_type spatial;
render_mode unshaded, cull_disabled, blend_add;

uniform vec4 beam_color : source_color = vec4(1.0, 0.95, 0.8, 1.0);
uniform float pulse_speed : hint_range(0.0, 10.0) = 3.0;
uniform float noise_density : hint_range(0.0, 50.0) = 20.0;
uniform float beam_intensity : hint_range(0.0, 5.0) = 2.0;

float hash(float n) {
    return fract(sin(n) * 43758.5453123);
}

void vertex() {
    float jitter = hash(TIME * 30.0 + float(VERTEX_ID)) * 0.02;
    VERTEX += NORMAL * jitter;
}

void fragment() {
    float scroll = UV.x * noise_density - (TIME * pulse_speed);
    float noise = smoothstep(0.4, 0.6, sin(scroll) * hash(floor(scroll)));
    
    float fresnel = pow(1.0 - dot(NORMAL, VIEW), 3.0);
    float flicker = mix(0.8, 1.2, hash(TIME * 20.0));
    vec3 final_glow = beam_color.rgb * (fresnel + noise * 0.5) * beam_intensity * flicker;
    
    ALBEDO = final_glow;
    ALPHA = (fresnel + noise) * beam_color.a;
}