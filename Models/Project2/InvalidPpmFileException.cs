using System;

namespace GrafikaKomputerowa.Models.Project2
{
    public class InvalidPpmFileException : Exception
    {
        public InvalidPpmFileException()
        {
        }

        public InvalidPpmFileException(string message) : base(message)
        {
        }
    }
}
