namespace music_service.Logging
{
    public static class DbLoggerExtentions
    {
        public static ILoggingBuilder AddDbLogger(this ILoggingBuilder builder,
            Action<DbLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
