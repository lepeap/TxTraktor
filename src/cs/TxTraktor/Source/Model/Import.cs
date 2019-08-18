namespace TxTraktor.Source.Model
{
    internal class Import
    {
        public Import()
        {
            
        }
        

        public Import(string source, 
            ImportType type,
            string name=null, 
            string localName=null)
        {
            Source = source;
            Name = name;
            Type = type;
            LocalName = string.IsNullOrEmpty(localName) ? name : localName;
        }
        public string Source { get; set; }
        public string Name { get; set; }
        public string LocalName {get; set; }
        public ImportType Type { get; set; }
    }
}