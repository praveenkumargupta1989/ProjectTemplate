using System.Data;

namespace ACIPL.Template.Server.DataAccess
{
    public class Parameter
    {
        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public Parameter(string name, object value, ParameterDirection parameterDirection)
        {
            Name = name;
            Value = value;
            ParameterDirection = parameterDirection;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public DbType DbType { get; set; }
        public ParameterDirection ParameterDirection { get; set; }
        public int Size { get; set; }
    }
}