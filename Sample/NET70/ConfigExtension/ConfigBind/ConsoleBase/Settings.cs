using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBase
{
    public sealed class Settings
    {
        public required int KeyOne { get; set; }
        public required bool KeyTwo { get; set; }
        public required NestedSettings KeyThree { get; set; } = null!;
    }
}
