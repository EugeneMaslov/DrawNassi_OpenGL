using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace DrawNassiOpenGL.DNDEngine.Shaders
{
    /// <summary>
    /// Простой класс шейдера для облегчения работы с шейдерами GLSL
    /// </summary>
    public class Shader
    {
        /// <summary>
        /// ID шейдера
        /// </summary>
        public readonly int Handle;

        /// <summary>
        /// Словарь локаций Uniform шейдера
        /// </summary>
        private readonly Dictionary<string, int> _uniformLocations;

        /// <summary>
        /// Создание простого шейдера
        /// </summary>
        /// <param name="vertPath">Шейдер вершин</param>
        /// <param name="fragPath">Шейдер фрагментов</param>
        public Shader(string vertPath, string fragPath)
        {
            // считывание шейдера вертексов
            var shaderSource = File.ReadAllText(vertPath);

            // создание шейдера
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexShader, shaderSource);

            // Компиляция шейдера
            CompileShader(vertexShader);

            // считывание шейдера фрагментов
            shaderSource = File.ReadAllText(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            Handle = GL.CreateProgram();

            // Применение шейдера
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            // Отключение шейдера
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            // Локации Uniform в шейдерах 
            _uniformLocations = new Dictionary<string, int>();

            // Добавление ключей
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
        }

        /// <summary>
        /// Компиляция шейдера
        /// </summary>
        /// <param name="shader">ID шейдера</param>
        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        /// <summary>
        /// Привязывание программы
        /// </summary>
        /// <param name="program">ID программы</param>
        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        /// <summary>
        /// Использовать шейдер
        /// </summary>
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        // Получить локацию атрибута
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        /// <summary>
        /// Изменяет Uniform шейдера
        /// </summary>
        /// <param name="name">Название формы</param>
        /// <param name="data">Данные для изменения</param>
        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Изменяет Uniform шейдера по float
        /// </summary>
        /// <param name="name">Название формы</param>
        /// <param name="data">Данные для изменения</param>
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Изменяет форму матрицы в шейдере
        /// </summary>
        /// <param name="name">Название формы</param>
        /// <param name="data">Данные для изменения</param>
        /// <remarks>
        ///   <para>
        ///     Матрица транспонируется перед отправкой в шейдер.
        ///   </para>
        /// </remarks>
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        /// <summary>
        /// Изменение вектора в шейдере
        /// </summary>
        /// <param name="name">Название формы</param>
        /// <param name="data">Данные для изменения</param>
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }
    }
}
