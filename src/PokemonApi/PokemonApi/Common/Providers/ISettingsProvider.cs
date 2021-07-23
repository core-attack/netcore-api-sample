namespace PokemonApi.Common.Providers
{
    public interface ISettingsProvider
    {
        string ConnectionString();

        string DefaultCsvFilePath();

        int BatchSize();
    }
}
