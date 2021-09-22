namespace SqlParserWrapper.Model
{
    public class ConvertCallEntity
    {
        public FunctionCallEntity ParameterFunctionCall { get; set;}
        public ParameterEntity Parameter { get; set;}
        public SqlDataTypeEntity SqlDataType { get; set;}
    }
}