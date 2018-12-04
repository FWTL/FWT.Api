using System;
using System.Collections.Generic;
using System.Text;
using OpenTl.Schema;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class File
    {
        public int Size { get;  set; }
        public IInputFileLocation Location { get; set; }
    }
}
