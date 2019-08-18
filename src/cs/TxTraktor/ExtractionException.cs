using System;

namespace TxTraktor
{
    public class ExtractionException: Exception
    {
        public ExtractionException(string message) :
            base(message)
        {
            
        }
    }
}