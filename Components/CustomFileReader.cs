using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hades_Map_Editor.Components
{
    internal class CustomFileReader
    {
        private string _buffer;
        public int _pointer;

        public CustomFileReader(StreamReader stream) {
            _buffer = stream.ReadToEnd();
            _buffer = _buffer.Replace("\r\n", "\n");
            _pointer = 0;
        }

        public char Read()
        {
            char val = _buffer[_pointer];
            _pointer++;

            return val;
        }
        public string Read(int size)
        {
            string val = "";
            val = _buffer.Substring(_pointer, size);
            _pointer += size;

            return val;
        }

        public char Peek()
        {
            return _buffer[_pointer];
        }
        public string Peek(int size)
        {
            string result = "";
            for(int i = 0; i < size; i++)
            {
                int index = _pointer + i;
                if(index >= _buffer.Length)
                {
                    result += "";
                }
                else
                {
                    result += _buffer[index];
                }
            }
            return result;
        }

        public void Seek(int position)
        {
            _pointer = position;
            if (_pointer < 0)
            {
                _pointer = 0;
            }
            if (_pointer >= _buffer.Length)
            {
                _pointer = _buffer.Length - 1;
            }
        }
        public void Skip(int offset)
        {
            _pointer += offset;
            if (_pointer < 0)
            {
                _pointer = 0;
            }
            if (_pointer >= _buffer.Length)
            {
                _pointer = _buffer.Length - 1;
            }
        }
        public bool good()
        {
            return _pointer < _buffer.Length;
        }
    }
}
