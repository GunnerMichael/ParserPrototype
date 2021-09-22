using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlParserWrapper;
using SqlParserWrapper.Contract;
using SqlParserWrapper.Model;

namespace SqlParserWrapper
{
    public class SqlParser
    {
        private IQueryProcesser _processer;
        private StreamWriter _debug;

        public SqlParser(IQueryProcesser queryProcesser, StreamWriter debug = null)
        {
            _processer = queryProcesser;
            _debug = debug;
        }

        public ParsedContainer Parser(string sql)
        {
            TSqlParser parser = new TSql120Parser(true);
            IList<ParseError> parseErrors;

            TSqlFragment sqlFragment = parser.Parse(new StringReader(sql), out parseErrors);

            if (parseErrors.Count > 0)
            {
                if (_debug != null)
                {
                    _debug.WriteLine("Errors parsing SQL");
                    foreach(var item in parseErrors)
                    {
                        _debug.WriteLine($"Line: {item.Line} Message: {item.Message}");
                    }
                }

                return null;
            }
            else
            {
                var comments =
                    (from x in sqlFragment.ScriptTokenStream
                     where x.TokenType == TSqlTokenType.MultilineComment
                     select x).ToList();

                List<TSqlParserTokenEntity> commentList = ConvertComments(comments);

             
                OwnVisitor visitor = new OwnVisitor(_processer, _debug);
                sqlFragment.Accept(visitor);

                return _processer.BuildOutput(commentList);
            }


        }

        private List<TSqlParserTokenEntity> ConvertComments(List<TSqlParserToken> comments)
        {
            List<TSqlParserTokenEntity> items = new List<TSqlParserTokenEntity>();

            foreach(var item in comments)
            {
                items.Add(new TSqlParserTokenEntity()
                {
                    Text = item.Text
                }
                );
            }

            return items;
        }
    }
}
