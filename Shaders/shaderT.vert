#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec2 aTexCoords;

out vec2 TexCoords;

uniform mat4 model;
uniform mat4 position;


void main()
{
	TexCoords = vec2(aTexCoords);
	// see how we directly give a vec3 to vec4's constructor
    gl_Position = vec4(aPosition, 1.0) * model * position;
}
