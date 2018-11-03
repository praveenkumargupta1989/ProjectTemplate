namespace ACIPL.Template.Server.Models
{
    public class DBStructure
    {
        public string TableName { get; set; }
        public bool IsMaster { get; set; }
        public string TableStructure { get; set; }
        public string SelectStmnt { get; set; }
        public string InsertStmnt { get; set; }
        public string ColumnsName { get; set; }
        public string WhereStmnt { get; set; }
        public string Columns { get; set; }
        public string Childtable { get; set; }
        public string Priority { get; set; }
        public string RelatedColumns { get; set; }
        public string RequiredColumn { get; set; }
        public bool Active { get; set; }
        public string WhereCondition { get; set; }
        public int TransferredGroupId { get; set; }

    }
}
