using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class FromClauseEntity
    {
        private List<TableEntity> _tables = new List<TableEntity>();
        public QualifiedJoinEntity QualifiedJoin { get; set;}

        public List<TableEntity> Tables
        {
            get
            {
                return _tables;
            }
        }

        public TableEntity FindTableFromAlias(string alias, bool includeAll = false)
        {
            TableEntity result = null;

            var uniqueTables = GetUniqueTables();

            var matched = (from t in uniqueTables
                          where t.TableReference != null && t.TableReference.NamedTable != null
                          && t.TableReference.NamedTable.Alias.Identifer != null
                          && t.TableReference.NamedTable.Alias.Identifer.GetValue().ToLower() == alias.ToLower()
                          select t).ToList();



            if (matched.Count() == 1)
            {

                // found the full name of the alias
                // but if that full name is used more than once, we have to use alias
                // unless we have asked to includeAll

                var x = (from t in uniqueTables
                               where t.TableReference != null && t.TableReference.NamedTable != null
                               && t.TableReference.NamedTable.SchemaObject != null
                               && t.TableReference.NamedTable.SchemaObject.FullName.ToLower() == matched[0].TableReference.NamedTable.SchemaObject.FullName.ToLower()
                               select t).ToList();

                if (x.Count == 1 || (x.Count > 0 && includeAll == true))
                {
                    
                    result = matched[0];
                }
                else
                {

                }
            }
            else if (matched.Count > 1)
            {
                Debug.WriteLine("Multiple tables have the alias");
            }



            return result;
        }

        public List<TableEntity> GetUniqueTables()
        {
            List<TableEntity> tables = new List<TableEntity>();

            if (QualifiedJoin != null)
            {
                tables.AddRange(GetTablesFromQualifiedJoin(source: QualifiedJoin.First));
                tables.AddRange(GetTablesFromQualifiedJoin(source: QualifiedJoin.Second));
            }


            if (_tables.Count > 0)
            {
                tables.AddRange(_tables);
            }

            var remove = (from t in tables
                         where t.TableReference is null || t.TableReference.NamedTable == null 
                         select t).ToList();

            foreach(var item in remove)
                tables.Remove(item);

            // add QueryDeriviedTables

            var qdt = (from t in _tables
                       where t.QueryDerivedTable != null
                       select t).ToList();

            //if (qdt.Count > 0)
            //{
            //    tables.AddRange(QueryDerivedTableEntity(tables: qdt));
            //}

            List<TableEntity> distinctTables = tables
                .GroupBy(t => new { t.TableReference.NamedTable.SchemaObject.FullName, t.TableReference.NamedTable.Alias.AliasName})
                .Select(g => g.FirstOrDefault())
                .ToList();

            return distinctTables;

        }

        private IEnumerable<TableEntity> QueryDerivedTableEntity(List<TableEntity> tables)
        {
            foreach(var item in tables)
            {
                // item.QueryDerivedTable.QueryExpression.QuerySpecification
            }

            return null;
        }

        private IEnumerable<TableEntity> GetTablesFromQualifiedJoin(TableEntity source)
        {
            List<TableEntity> tables = new List<TableEntity>();

            if (source.QualifiedJoin is not null)
            {
                tables.AddRange(GetTablesFromQualifiedJoin(source.QualifiedJoin.First));
                tables.AddRange(GetTablesFromQualifiedJoin(source.QualifiedJoin.Second));
            }

            if (source.TableReference is not null)
            {              
                tables.Add(new TableEntity() { TableReference = source.TableReference });
            }

            return tables;
        }
    }
}
