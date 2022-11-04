namespace SharedModels.QueryStringParameters;

public class CountryParameters : QueryStringParameters
{
    public const string DefaultFields = "id,code,name";
    
    public CountryParameters()
    {
        Sort = "id";
        Fields = DefaultFields;
    }
    
    public string? Code { get; set; }
    public string? Name { get; set; }
}