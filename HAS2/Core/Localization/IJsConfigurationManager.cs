namespace HAS2.Core.Localization
{
    public interface IJsConfigurationScriptManager : IJsResourceScriptManager
    {
        string Prefix { get; set; }

        string VariableName { get; set; }
    }
}